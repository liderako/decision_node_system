using System;
using System.Collections.Generic;
using DecisionNS.Data;
using DecisionNS.Data.Error;
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
    public class DNSEventNode : DNSNode
    {
        private List<DNSEventItem> nodeItems { get; set; }
        private List<ObjectField> scriptableObjectField;

        private bool isError;
        private int indexError;
        
        public override void Initialize(int id, Vector2 position, DNSGraphView graph)
        {
            base.Initialize(id, position, graph);

            NodeName = "Event Node";
            
            Type = DNSTypes.Event;
            
            PortLink.Add(new DNSPortLink());
            
        }

        public override void Draw()
        {
            base.Draw();
            
            mainContainer.ClearClassList();
            mainContainer.AddToClassList("dns-node-event__main-container");

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
                ActivateErrorNode();
                
                ObjectField field = CreateEventField(null);
                field.AddToClassList("dns_node_event__object_field");
                scriptableObjectField.Add(field);
                CustomDataContainer.Add(field);
                RegisterFieldCallBack(field);

            }).Setup(title:"Add Event Item");
            addFieldButton.AddToClassList("dns_node__button");
            extensionContainer.Insert(0, addFieldButton);
        }

        private void RegisterFieldCallBack(ObjectField field)
        {
            field.RegisterCallback<ChangeEvent<Object>>((evt) =>
            {
                if (evt.newValue is null && !isError)
                {
                    ActivateErrorNode();
                }
                else if (evt.newValue is not null && isError)
                {
                    DeactivateErrorNode();
                }
            });
        }

        private void ActivateErrorNode()
        {
            indexError++;
            isError = true;
            SetErrorStyle(DNSErrorData.Color);
            graphView.IncreaseIndexError();
        }

        private void DeactivateErrorNode()
        {
            indexError--;
            if (indexError <= 0)
            {
                indexError = 0;
                isError = false;
                ResetStyle();   
            }
            graphView.DecreaseIndexError();
        }


        private void DeleteButton(ObjectField visualElement, ScriptableObject obj)
        {
            Button deleteFieldButton = new Button((() =>
            {
                if (scriptableObjectField.Count == 1)
                {
                    return;
                }
                if (visualElement.value == null && isError)
                {
                    DeactivateErrorNode();
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
                nodeItems = new List<DNSEventItem>();
            }
            scriptableObjectField = new List<ObjectField>();
            
            foreach (var eventNodeData in nodeItems)
            {
                ObjectField field = CreateEventField(eventNodeData);
                if (field.value == null)
                {
                    ActivateErrorNode();
                }
                RegisterFieldCallBack(field);
                scriptableObjectField.Add(field);
                CustomDataContainer.Add(field);
            }
        }

        private ObjectField CreateEventField(DNSEventItem obj)
        {
            ObjectField field = new ObjectField("Event Item")
            {
                objectType = typeof(DNSEventItem),
                allowSceneObjects = false,
                value = obj
            };
            DeleteButton(field, obj);
            return field;
        }
        
        public override DNode GetSaveData(DNode dNode=null)
        {
            if (dNode == null)
            {
                EventNode eventNode = new EventNode();
                base.GetSaveData(eventNode);
                eventNode.EventItems = GetNodes();
                return eventNode;
            }
            if (dNode is EventNode)
            {
                var a = (EventNode)dNode;
                a.EventItems = GetNodes();
                return a;
            }
            throw new Exception("Wrong type");
        }

        private List<DNSEventItem> GetNodes()
        {
            List<DNSEventItem> nodes = new List<DNSEventItem>();
            foreach (var field in scriptableObjectField)
            {
                nodes.Add((DNSEventItem)field.value);
            }
            return nodes;
        }

        public override void UploadSaveData(DNode node)
        {
            if (node is not EventNode)
            {
                throw new Exception("Wrong type: EventDNode");
            }
            base.UploadSaveData(node);
            EventNode n = (EventNode)node;
            nodeItems = n.EventItems;
        }
    }
}