using DecisionNS.Enumerations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DecisionNS.Elements
{
    public class DNSMultipleChoiceNode : DNSNode
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);

            Type = DNSTypes.MultipleChoice;
            
            Choices.Add("New Choice");
        }

        public override void Draw()
        {
            base.Draw();
            
            // note: main container
            Button addChoice = new Button()
            {
                text = "Add"
            };
            
            addChoice.AddToClassList("dns_node__button");
            
            mainContainer.Insert(1, addChoice);
            
            // note: output container
            
            foreach (var choice in Choices)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));

                choicePort.portName = "";

                Button deleteChoice = new Button()
                {
                    text = "Delete"
                };
                
                deleteChoice.AddToClassList("dns_node__button");

                TextField textField = new TextField()
                {
                    value = choice
                };
                textField.AddToClassList("dns-node__choice-textfield");
                textField.AddToClassList("dns-node__filename-textfield");
                textField.AddToClassList("dns-node__textfield__hidden");

                choicePort.Add(textField);
                choicePort.Add(deleteChoice);
                
                outputContainer.Add(choicePort);
            }
            
            RefreshExpandedState();
        }
    }
}