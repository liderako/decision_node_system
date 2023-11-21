using System;
using DecisionNS.Data.Save;
using DecisionNS.Editor.DecisionNodeSystem.Data.ScriptableObjects;
using DecisionNS.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace DecisionNS.Elements
{
    using Enumerations;
    
    public class DNSDecisionNode : DNSNode
    {
        public bool isNpc { get; set; }
        public ScriptableObject nodeItem { get; set; }

        private ObjectField scriptableObjectField;
        private Toggle IsNPC;

        public override void Initialize(int id, Vector2 position, DNSGraphView graph)
        {
            base.Initialize(id, position, graph);

            NodeName = "Decision Node";
            
            Type = DNSTypes.Decision;
            
            Choices.Add(new DNSChoiceSaveData() { Text = "Output" });
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

        private void CustomDataContainerSetup()
        {
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
                if (evt.newValue is DialogueConfig)
                {
                    textArea.value = ((DialogueConfig)evt.newValue).text;
                }
                nodeItem = (ScriptableObject)evt.newValue;
            });
            scriptableObjectField.AddToClassList("dns-node__element_in_single_node");
            
            IsNPC = new Toggle();
            IsNPC.label = "IS NPC";
            IsNPC.value = isNpc;
            IsNPC.AddToClassList("dns-node__element_in_single_node");
            IsNPC.AddToClassList("dns_node_toggle");
            
            visualElement.Add(scriptableObjectField);
            visualElement.Add(IsNPC);

            CustomDataContainer.Add(visualElement);
        }


        public override DNode GetSaveData(DNode dNode=null)
        {
            if (dNode == null)
            {
                SingleDNode singleDNode = new SingleDNode();
                base.GetSaveData(singleDNode);
                singleDNode.Fill(isNpc, nodeItem);
                return singleDNode;
            }
            if (dNode is SingleDNode)
            {
                var a = (SingleDNode)dNode;
                a.Fill(isNpc, nodeItem);
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
        }
    }
}