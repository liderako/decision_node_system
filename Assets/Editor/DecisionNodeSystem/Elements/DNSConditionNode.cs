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
        private List<(ObjectField, Toggle)> scriptableObjectField;
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
                
                var field = CreateField(null);
                field.Item1.AddToClassList("dns_node_event__object_field");
                scriptableObjectField.Add(field);
                CustomDataContainer.Add(field.Item1);
                ErrorNodeComponent.RegisterFieldCallBack(field.Item1);
            }).Setup(title:"Add Condition Item");
            addFieldButton.AddToClassList("dns_node__button");
            extensionContainer.Insert(0, addFieldButton);
        }
        
        private void DeleteButton(ObjectField visualElement, Toggle bToggle)
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
                scriptableObjectField.Remove((visualElement, bToggle));
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
            scriptableObjectField = new List<(ObjectField, Toggle)>();
            
            foreach (var eventNodeData in nodeItems)
            {
                var field = CreateField(eventNodeData);
                if (field.Item1.value == null)
                {
                    ErrorNodeComponent.ActivateErrorNode();
                }
                ErrorNodeComponent.RegisterFieldCallBack(field.Item1);
                scriptableObjectField.Add(field);
                CustomDataContainer.Add(field.Item1);
            }
        }

        private (ObjectField, Toggle) CreateField(DNSConditionItem obj)
        {
            ObjectField field = new ObjectField()
            {
                objectType = typeof(DNSConditionItem),
                allowSceneObjects = false,
                value = obj
            };

            field.AddToClassList("dns-node__object_field");

            Toggle statusToggle = new Toggle();
            statusToggle.value = obj == null ? false : obj.Status;

            field.Insert(0, statusToggle);
            
            DeleteButton(field, statusToggle);
            
            return (field, statusToggle);
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
                DNSConditionItem value = (DNSConditionItem)field.Item1.value;
                value.Status = field.Item2.value;
                nodes.Add(value);
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