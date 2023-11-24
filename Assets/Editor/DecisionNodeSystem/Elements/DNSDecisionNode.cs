using System;
using DecisionNS.Data;
using DecisionNS.Editor.DecisionNodeSystem.Data.ScriptableObjects;
using DecisionNS.Enums;
using DecisionNS.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace DecisionNS.Elements
{
    public class DNSDecisionNode : DNSNode
    {
        private bool isNpc { get; set; }
        private ScriptableObject nodeItem { get; set; }
        private string text;
        
        // UIelements
        private ObjectField scriptableObjectField;
        private Toggle IsNPCToggle;
        private TextField textArea;

        public override void Initialize(int id, Vector2 position, DNSGraphView graph)
        {
            base.Initialize(id, position, graph);

            NodeName = "Decision Node";
            
            Type = DNSTypes.Decision;
            
            PortLink.Add(new DNSPortLink());
        }

        public override void Draw()
        {
            base.Draw();
            OutputContainerSetup();
            CustomDataContainerSetup();
            RefreshExpandedState();
        }

        private void OutputContainerSetup()
        {
            Port choicePort =  this.CreateOutput(Port.Capacity.Multi, name:"Output");

            outputContainer.Add(choicePort);
        }

        private void SetupTextArea()
        {
            Foldout textFoldout = new Foldout().CreateFoldout("Decision Text");

            textArea = new TextField().CreateTextArea("Decision text");
            textArea.AddToClassList("dns-node__textfield");
            textArea.AddToClassList("dns-node__filename-textfield");
            textArea.AddToClassList("dns-node__textfield__hidden");
            textArea.value = text;

            textFoldout.Add(textArea);
            CustomDataContainer.Add(textFoldout);   
        }

        private void CustomDataContainerSetup()
        {
            SetupTextArea();
            
            VisualElement visualElement = new VisualElement();
            visualElement = new VisualElement();
            visualElement.AddToClassList("dns-node__single-decision-container");
            
            scriptableObjectField = new ObjectField("Node Item")
            {
                objectType = typeof(BaseConfig),
                allowSceneObjects = false,
                value = nodeItem
            };
            scriptableObjectField.RegisterCallback<ChangeEvent<Object>>((evt) =>
            {
                if (evt.newValue is DialogueItem)
                {
                    textArea.value = ((DialogueItem)evt.newValue).text;
                }
                nodeItem = (ScriptableObject)evt.newValue;
            });
            scriptableObjectField.AddToClassList("dns-node__element_in_single_node");
            
            IsNPCToggle = new Toggle();
            IsNPCToggle.label = "IS NPC";
            IsNPCToggle.value = isNpc;
            IsNPCToggle.AddToClassList("dns-node__element_in_single_node");
            IsNPCToggle.AddToClassList("dns_node_toggle");
            
            visualElement.Add(scriptableObjectField);
            visualElement.Add(IsNPCToggle);

            CustomDataContainer.Add(visualElement);
        }


        public override DNode GetSaveData(DNode dNode=null)
        {
            if (dNode == null)
            {
                SingleDNode singleDNode = new SingleDNode();
                base.GetSaveData(singleDNode);
                singleDNode.Fill(isNpc, nodeItem, textArea.value);
                return singleDNode;
            }
            if (dNode is SingleDNode)
            {
                var a = (SingleDNode)dNode;
                a.Fill(isNpc, nodeItem, textArea.value);
                return dNode;
            }
            throw new Exception("Wrong type");
        }
        
        public override void UploadSaveData(DNode node)
        {
            if (node is not SingleDNode)
            {
                throw new Exception("Wrong type: SingleDNode");
            }
            base.UploadSaveData(node);
            SingleDNode n = (SingleDNode)node;
            isNpc = n.IsNPC;
            nodeItem = n.NodeItem;
            text = n.Text;
        }
    }
}