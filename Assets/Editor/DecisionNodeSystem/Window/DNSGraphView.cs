using System;
using System.Collections.Generic;
using DecisionNS.Elements;
using DecisionNS.Enumerations;
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
        public DNSGraphView(DNSEditorWindow editorWindow)
        {
            this.editorWindow = editorWindow;
            
            AddManipulators();
            AddGridBackground();
            AddSearchWindow();
            
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
            
            // this.AddManipulator(CreateNodeContextMenu( "Add Node (Single Choice)", DNSTypes.SingleChoice));
            // this.AddManipulator(CreateNodeContextMenu("Add Node (Multiple Choice)" , DNSTypes.MultipleChoice));

            this.AddManipulator(CreateGroupContextMenu());
        }
        
        private IManipulator CreateGroupContextMenu()
        {
            ContextualMenuManipulator menuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", 
                    actionEvent => AddElement(CreateGroup("New Group",
                        GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))));
            return menuManipulator;
        }

        // private IManipulator CreateNodeContextMenu(string actionTitle, DNSTypes type)
        // {
        //     ContextualMenuManipulator menuManipulator = new ContextualMenuManipulator(
        //         menuEvent => menuEvent.menu.AppendAction(actionTitle, 
        //             actionEvent => AddElement(CreateNode(type, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))));
        //     return menuManipulator;
        // }

        public GraphElement CreateGroup(string title, Vector2 position)
        {
            Group group = new Group()
            {
                title = title,
            };
            group.SetPosition(new Rect(position, Vector2.zero));
            return group;
        }

        public DNSNode CreateNode(DNSTypes decisionType, Vector2 position)
        {
            Type type = Type.GetType($"DecisionNS.Elements.DNS{decisionType}Node");

            DNSNode node = (DNSNode)Activator.CreateInstance(type);
            node.Initialize(position);
            node.Draw();
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
                searchWindow.Initialize(this);
            }

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(GetLocalMousePosition(context.screenMousePosition)), searchWindow);
        }
        
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo(editorWindow.rootVisualElement.parent, mousePosition - editorWindow.position.position);
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }
    }
}
