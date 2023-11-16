using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

        public virtual void Initialize(Vector2 position)
        {
            DecisionName = "DecisionName";
            Choices = new List<string>();
            Text = "Decision text";
            
            SetPosition(new Rect(position, Vector2.zero));
        }

        public virtual void Draw()
        {
            // note: title container
            TextField decisionNameTextField = new TextField()
            {
                value = DecisionName
            };
            titleContainer.Insert(0, decisionNameTextField);

            // note: input
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Decision Connection";
            
            inputContainer.Add(inputPort);

            // note extensions container

            VisualElement customDataContainer = new VisualElement();
            
            Foldout textFoldout = new Foldout()
            {
                text = "Decision Text"
            };

            TextField textField = new TextField()
            {
                value = Text
            };
            
            textFoldout.Add(textField);
            
            customDataContainer.Add(textFoldout);
            
            extensionContainer.Add(customDataContainer);
        }
    }
}