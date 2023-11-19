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
    
    public class DNSNode : Node
    {
        public Int64 Id;
        public string DecisionName { get; set; }
        public List<DNSChoiceSaveData> Choices { get; set; }
        public string Text { get; set; }
        public DNSTypes Type { get; set; }

        protected DNSGraphView graphView;
        
        private StyleColor originBackgroundColor;

        // public ScriptableObject TestSo;

        public virtual void Initialize(int id, Vector2 position, DNSGraphView graph)
        {
            Id = id;
            originBackgroundColor = mainContainer.style.backgroundColor;

            graphView = graph;
            
            DecisionName = "BaseNode";
            Choices = new List<DNSChoiceSaveData>();
            Text = "Decision text";
            
            SetPosition(new Rect(position, Vector2.zero));
            
            extensionContainer.AddToClassList("dns-node__extension-container");
            extensionContainer.AddToClassList("dns-node__main-container");
        }

        public virtual void Draw()
        {
            // note: title container
            TextField decisionNameTextField = new TextField().CreateTextField(DecisionName);
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

            // note: input
            Port inputPort = this.CreateInput(Port.Capacity.Multi);
            inputContainer.Add(inputPort);

            // note extensions container

            VisualElement customDataContainer = new VisualElement();
            
            customDataContainer.AddToClassList("dns-node__custom-data-container");

            Foldout textFoldout = new Foldout().CreateFoldout("Decision Text");

            TextField textArea = new TextField().CreateTextArea(Text);
            textArea.AddToClassList("dns-node__textfield");
            textArea.AddToClassList("dns-node__filename-textfield");
            textArea.AddToClassList("dns-node__textfield__hidden");
            
            // ObjectField scriptableObjectField = new ObjectField("ScriptableObject")
            // {
            //     objectType = typeof(ScriptableObject),
            //     allowSceneObjects = false,
            //     value = TestSo
            // };
            
            textFoldout.Add(textArea);
            customDataContainer.Add(textFoldout);
            // customDataContainer.Add(scriptableObjectField);

            extensionContainer.Add(customDataContainer);
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = originBackgroundColor;
        }
    }
}