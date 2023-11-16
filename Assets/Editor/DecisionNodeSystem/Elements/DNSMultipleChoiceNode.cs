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
            mainContainer.Insert(1, addChoice);
            
            // note: output container
            
            foreach (var choice in Choices)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
                    typeof(bool));

                choicePort.portName = "";

                Button deleteChoice = new Button()
                {
                    text = "Delete"
                };

                TextField textField = new TextField()
                {
                    value = choice
                };
                
                choicePort.Add(textField);
                choicePort.Add(deleteChoice);
                
                outputContainer.Add(choicePort);
            }
            
            RefreshExpandedState();
        }
    }
}