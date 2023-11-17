using System.Collections.Generic;
using RMGames;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DecisionNS.Elements
{
    using Enumerations;
    
    public class DNSNode : Node
    {
        public string DecisionName { get; set; }
        public List<string> Choices { get; set; }
        public string Text { get; set; }
        public DNSTypes Type { get; set; }

        public ScriptableObject TestSo;

        public virtual void Initialize(Vector2 position)
        {
            DecisionName = "DecisionName";
            Choices = new List<string>();
            Text = "Decision text";
            
            SetPosition(new Rect(position, Vector2.zero));
            
            extensionContainer.AddToClassList("dns-node__extension-container");
            extensionContainer.AddToClassList("dns-node__main-container");
        }

        public virtual void Draw()
        {
            // note: title container
            TextField decisionNameTextField = new TextField()
            {
                value = DecisionName
            };

            decisionNameTextField.AddToClassList("dns-node__textfield");
            decisionNameTextField.AddToClassList("dns-node__filename-textfield");
            decisionNameTextField.AddToClassList("dns-node__textfield__hidden");
            
            titleContainer.Insert(0, decisionNameTextField);

            // note: input
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Input";
            
            inputContainer.Add(inputPort);

            // note extensions container

            VisualElement customDataContainer = new VisualElement();
            
            customDataContainer.AddToClassList("dns-node__custom-data-container");
            
            Foldout textFoldout = new Foldout()
            {
                text = "Decision Text"
            };

            TextField textField = new TextField()
            {
                value = Text
            };
            textField.AddToClassList("dns-node__textfield");
            textField.AddToClassList("dns-node__filename-textfield");
            textField.AddToClassList("dns-node__textfield__hidden");
            
            ObjectField scriptableObjectField = new ObjectField("ScriptableObject")
            {
                objectType = typeof(ScriptableObject),
                allowSceneObjects = false,
                value = TestSo
            };
            
            textFoldout.Add(textField);
            customDataContainer.Add(textFoldout);
            customDataContainer.Add(scriptableObjectField);

            extensionContainer.Add(customDataContainer);
        }
    }
}