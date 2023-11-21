using System;
using System.Collections.Generic;
using DecisionNS.Data.Save;
using DecisionNS.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DecisionNS.Elements
{
    using Enumerations;
    
    public abstract class DNSNode : Node
    {
        public Int64 Id { get; set; }
        public string NodeName { get; set; }

        public string Text;
        
        public List<DNSChoiceSaveData> Choices { get; set; }
        public DNSTypes Type { get; set; }

        protected DNSGraphView graphView;
        
        private StyleColor originBackgroundColor;
        protected VisualElement CustomDataContainer { get; set; }

        protected TextField textArea;


        public virtual void Initialize(int id, Vector2 position, DNSGraphView graph)
        {
            Id = id;
            originBackgroundColor = mainContainer.style.backgroundColor;

            graphView = graph;
            
            NodeName = "BaseNode";
            Choices = new List<DNSChoiceSaveData>();

            SetPosition(new Rect(position, Vector2.zero));
            
            extensionContainer.AddToClassList("dns-node__extension-container");
            extensionContainer.AddToClassList("dns-node__main-container");
        }

        public virtual void Draw()
        {
            TitleContainerSetup();
            InputPortSetup();
            ExtensionsContainer();
        }

        private void TitleContainerSetup()
        {
            TextField decisionNameTextField = new TextField().CreateTextField(NodeName);
            decisionNameTextField.isReadOnly = true;
            decisionNameTextField.focusable = false;
            decisionNameTextField.AddToClassList("dns-node__textfield");
            decisionNameTextField.AddToClassList("dns-node__filename-textfield");
            decisionNameTextField.AddToClassList("dns-node__textfield__hidden");
            
            TextField idTextField = new TextField().CreateTextField(Id.ToString(), callback =>
            {
                graphView.RemoveUngroupedNode(this);

                Id = Int32.Parse(callback.newValue);

                graphView.AddUngroupedNode(this);
            });
            idTextField.AddToClassList("dns-node__id-textfield");
            idTextField.AddToClassList("dns-node__textfield__hidden");

            titleContainer.Insert(0, decisionNameTextField);
            titleContainer.Insert(1, idTextField);   
        }

        private void InputPortSetup()
        {
            Port inputPort = this.CreateInput(Port.Capacity.Multi);
            inputContainer.Add(inputPort);
        }
        
        private void ExtensionsContainer()
        {
            CustomDataContainer = new VisualElement();
            CustomDataContainer.AddToClassList("dns-node__custom-data-container");

            Foldout textFoldout = new Foldout().CreateFoldout("Decision Text");

            textArea = new TextField().CreateTextArea("Decision text");
            textArea.AddToClassList("dns-node__textfield");
            textArea.AddToClassList("dns-node__filename-textfield");
            textArea.AddToClassList("dns-node__textfield__hidden");
            textArea.value = Text;

            textFoldout.Add(textArea);
            CustomDataContainer.Add(textFoldout);

            extensionContainer.Add(CustomDataContainer);
        }
        
        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = originBackgroundColor;
        }

        public virtual DNode GetSaveData(DNode node=null)
        {
            if (node == null)
            {
                node = new DNode();
                node.Fill(Id, Type, textArea.value, GetPosition().position);
                // node.Choices = Choices;
            }
            else
            {
                node.Fill(Id, Type, textArea.value, GetPosition().position);
                List<DNSChoiceSaveData> choiceSaveData = new List<DNSChoiceSaveData>();
                foreach (Port port in outputContainer.Children())
                {
                    foreach (var edge in port.connections)
                    {
                        DNSNode nextNode = (DNSNode) edge.input.node;

                        // DNSChoiceSaveData choiceData = (DNSChoiceSaveData) edge.output.userData;

                        // choiceData.NodeID = nextNode.Id;
                        
                        choiceSaveData.Add(new DNSChoiceSaveData()
                        {
                            Text = "Output",
                            NodeID = nextNode.Id
                        });
                        
                    }
                }
                node.Choices = choiceSaveData;
            }
            return node;
        }

        public virtual void UploadSaveData(DNode node)
        {
            Id = node.Id;
            Text = node.Text;
            Choices = node.Choices;
        }
        
        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        private void DisconnectInputPorts()
        {
            DisconnectPorts(inputContainer);
        }

        private void DisconnectOutputPorts()
        {
            DisconnectPorts(outputContainer);
        }

        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (!port.connected)
                {
                    continue;
                }
                graphView.DeleteElements(port.connections);
            }
        }
    }
}