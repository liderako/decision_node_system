using System;
using System.Collections.Generic;
using DecisionNS.Data;
using DecisionNS.Editor.DecisionNodeSystem.Data.ScriptableObjects;
using DecisionNS.Editor.DecisionNodeSystem.Elements.Components;
using DecisionNS.Elements.Interfaces;
using DecisionNS.Enums;
using DecisionNS.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DecisionNS.Elements
{
    public class DNSConditionNode : DNSNode, IErrorNodeComponent
    {
        private List<DNSConditionItem> nodeItems { get; set; }
        private List<ObjectField> scriptableObjectField;
        
        public ErrorNodeComponent ErrorNodeComponent { get; set; }

        public override void Initialize(int id, Vector2 position, DNSGraphView graph)
        {
            base.Initialize(id, position, graph);
            NodeName = "Condition Node";
            Type = DNSTypes.Condition;
            PortLink.Add(new DNSPortLink());
            ErrorNodeComponent = new ErrorNodeComponent(this);
        }
        
        public override void Draw()
        {
            base.Draw();
            
            mainContainer.ClearClassList();
            mainContainer.AddToClassList("dns-node-condition__main-container");

            OutputContainerSetup();
            DrawObjectFields();
            AddButton();
            RefreshExpandedState();
        }
        
        private void OutputContainerSetup()
        {
            Port choicePort =  this.CreateOutput(Port.Capacity.Single, name:"Output");
            outputContainer.Add(choicePort);
        }
        
        private void AddButton()
        {
            Button addFieldButton = new Button(() =>
            {
                ErrorNodeComponent.ActivateErrorNode();
                
                ObjectField field = CreateField(null);
                field.AddToClassList("dns_node_event__object_field");
                scriptableObjectField.Add(field);
                CustomDataContainer.Add(field);
                ErrorNodeComponent.RegisterFieldCallBack(field);
            }).Setup(title:"Add Condition Item");
            addFieldButton.AddToClassList("dns_node__button");
            extensionContainer.Insert(0, addFieldButton);
        }
        
        private void DeleteButton(ObjectField visualElement)
        {
            Button deleteFieldButton = new Button((() =>
            {
                if (scriptableObjectField.Count == 1)
                {
                    return;
                }
                if (visualElement.value == null && ErrorNodeComponent.IsError)
                {
                    ErrorNodeComponent.DeactivateErrorNode();
                }
                scriptableObjectField.Remove(visualElement);
                CustomDataContainer.Remove(visualElement);
            })).Setup(title:"Delete");
            deleteFieldButton.AddToClassList("dns_node__button");
            visualElement.Add(deleteFieldButton);
        }
        
        private void DrawObjectFields()
        {
            if (nodeItems == null)
            {
                nodeItems = new List<DNSConditionItem>();
                nodeItems.Add(null);
            }
            scriptableObjectField = new List<ObjectField>();
            
            foreach (var eventNodeData in nodeItems)
            {
                ObjectField field = CreateField(eventNodeData);
                if (field.value == null)
                {
                    ErrorNodeComponent.ActivateErrorNode();
                }
                ErrorNodeComponent.RegisterFieldCallBack(field);
                scriptableObjectField.Add(field);
                CustomDataContainer.Add(field);
            }
        }

        private ObjectField CreateField(DNSConditionItem obj)
        {
            ObjectField field = new ObjectField("Condition Item")
            {
                objectType = typeof(DNSConditionItem),
                allowSceneObjects = false,
                value = obj
            };
            DeleteButton(field);
            return field;
        }
        
        public override DNode GetSaveData(DNode dNode=null)
        {
            if (dNode == null)
            {
                ConditionNode conditionNode = new ConditionNode();
                base.GetSaveData(conditionNode);
                conditionNode.ConditionItems = GetNodes();
                return conditionNode;
            }
            if (dNode is ConditionNode)
            {
                var a = (ConditionNode)dNode;
                a.ConditionItems = GetNodes();
                return a;
            }
            throw new Exception("Wrong type");
        }

        private List<DNSConditionItem> GetNodes()
        {
            List<DNSConditionItem> nodes = new List<DNSConditionItem>();
            foreach (var field in scriptableObjectField)
            {
                nodes.Add((DNSConditionItem)field.value);
            }
            return nodes;
        }

        public override void UploadSaveData(DNode node)
        {
            if (node is not ConditionNode)
            {
                throw new Exception("Wrong type: EventDNode");
            }
            base.UploadSaveData(node);
            ConditionNode n = (ConditionNode)node;
            nodeItems = n.ConditionItems;
        }

    }
}