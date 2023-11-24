using System;
using System.Collections.Generic;
using DecisionNS.Data.Error;
using DecisionNS.Data;
using DecisionNS.Elements;
using DecisionNS.Enums;
using DecisionNS.Utilities;
using DecisionNS.Windows;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DecisionNS
{
    public class DNSGraphView : GraphView
    {
        private DNSSearchWindow searchWindow;
        private DNSEditorWindow editorWindow;

        private SerializableDictionary<Int64, DNSNodeErrorData> ungroupsNodes;
        private int IndexError;

        public delegate void OnValueIndexError(int indexError);

        public OnValueIndexError onValueIndexError;

        public void IncreaseIndexError()
        {
            IndexError++;
            onValueIndexError(IndexError);
        }

        public void DecreaseIndexError()
        {
            IndexError--;
            if (IndexError <= 0)
            {
                IndexError = 0;
            }
            onValueIndexError(IndexError);  
        }

        public DNSGraphView(DNSEditorWindow editorWindow)
        {
            this.editorWindow = editorWindow;

            ungroupsNodes = new SerializableDictionary<Int64, DNSNodeErrorData>();

            AddManipulators();
            AddGridBackground();
            AddSearchWindow();

            OnElementsDeleted();
            AddStyle();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port) // if A it's A
                {
                    return;
                }

                if (startPort.node == port.node) // if input == input or output == output
                {
                    return;
                }

                if (startPort.direction == port.direction)
                {
                    return;
                }
                
                compatiblePorts.Add(port);
            });
            
            return compatiblePorts;
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContentDragger());
        }

        public DNSNode CreateNode(DNSTypes decisionType, Vector2 position, bool drawMode=true)
        {
            Type type = Type.GetType($"DecisionNS.Elements.DNS{decisionType}Node");

            DNSNode node = (DNSNode)Activator.CreateInstance(type);
            node.Initialize(node.GetHashCode(), position, this);
            if (drawMode)
            {
                node.Draw();   
            }

            AddUngroupedNode(node);
            
            return node;
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        private void AddStyle()
        {
            this.AddStyleSheets("DecisionNodeSystem/DNSGraphViewStyle.uss", "DecisionNodeSystem/DNSNodeStyle.uss");
        }
        
        private void AddSearchWindow()
        {
            if (searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<DNSSearchWindow>();
            }
            
            searchWindow.Initialize(this);

            // nodeCreationRequest = context => Debug.Log("CLICK");

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }

        private void OnElementsDeleted()
        {
            Type edgeType = typeof(Edge);
            
            deleteSelection = (operationName, askUser) =>
            {
                List<DNSNode> nodesToDelete = new List<DNSNode>();
                List<Edge> edgesToDelete = new List<Edge>();
                
                foreach (GraphElement element in selection)
                {
                    if (element is DNSNode)
                    {
                        nodesToDelete.Add((DNSNode)element);
                        continue;
                    }
                    
                    if (element.GetType() == edgeType)
                    {
                        Edge edge = (Edge) element;
                        edgesToDelete.Add(edge);
                        continue;
                    }
                }
                
                DeleteElements(edgesToDelete);

                for (int i = 0; i < nodesToDelete.Count; i++)
                {
                    RemoveUngroupedNode(nodesToDelete[i]);
                    nodesToDelete[i].DisconnectAllPorts();
                    RemoveElement(nodesToDelete[i]);
                }
            };
        }

        public void AddUngroupedNode(DNSNode node)
        {
            Int64 id = node.Id;
            if (!ungroupsNodes.ContainsKey(id))
            {
                DNSNodeErrorData nodeErrorData = new DNSNodeErrorData();
                nodeErrorData.Nodes.Add(node);
                ungroupsNodes.Add(id, nodeErrorData);
                return;
            }
            
            ungroupsNodes[id].Nodes.Add(node);
            
            node.SetErrorStyle(DNSErrorData.Color);

            if (ungroupsNodes[id].Nodes.Count >= 2)
            {
                ungroupsNodes[id].Nodes[0].SetErrorStyle(DNSErrorData.Color);
                IncreaseIndexError();
            }
        }
        
        public void RemoveUngroupedNode(DNSNode n)
        {
            if (ungroupsNodes.ContainsKey(n.Id))
            {
                ungroupsNodes[n.Id].Nodes.Remove(n);
                n.ResetStyle();

                if (ungroupsNodes[n.Id].Nodes.Count >= 1)
                {
                    ungroupsNodes[n.Id].Nodes[0].ResetStyle();
                    DecreaseIndexError();
                }

                if (ungroupsNodes[n.Id].Nodes.Count == 0)
                {
                    ungroupsNodes.Remove(n.Id);
                }
            }
        }

        public bool IsCanSave()
        {
            return false;
        }
        
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo(editorWindow.rootVisualElement.parent, mousePosition - editorWindow.position.position);
            }
            
            return contentViewContainer.WorldToLocal(worldMousePosition);
        }

        public List<DNode> GetNodesForSave()
        {
            List<DNode> nodes = new List<DNode>();
            graphElements.ForEach(graphElement =>
            {
                if (graphElement is DNSNode node)
                {
                    nodes.Add(node.GetSaveData());
                }
            });
            return nodes;   
        }
        
        public void ClearGraph()
        {
            graphElements.ForEach(graphElement => RemoveElement(graphElement));
            
            ungroupsNodes.Clear();
            IndexError = 0;
        }
    }
}
