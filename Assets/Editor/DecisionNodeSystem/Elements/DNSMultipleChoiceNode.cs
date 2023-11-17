using DecisionNS.Enumerations;
using DecisionNS.Utilities;
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

            DecisionName = "Multiple Decision Node";

            Type = DNSTypes.MultipleChoice;
            
            Choices.Add("New Choice");
        }

        public override void Draw()
        {
            base.Draw();
            
            // note: main container
            Button addChoice = new Button(() =>
            {
                Port port = CreatePort("New Choice");
                Choices.Add("New Choice");
                outputContainer.Add(port);
            }).Setup(title:"Add");
            addChoice.AddToClassList("dns_node__button");
            
            mainContainer.Insert(1, addChoice);
            
            // note: output container
            
            foreach (var choice in Choices)
            {
                Port choicePort = CreatePort(choice);
                outputContainer.Add(choicePort);
            }
            
            RefreshExpandedState();
        }

        private Port CreatePort(string choice)
        {
            Port choicePort = this.CreateOutput(Port.Capacity.Multi);
            Button deleteChoice = new Button().Setup(title:"Delete");
            deleteChoice.AddToClassList("dns_node__button");

            TextField textField = new TextField().CreateTextField(choice);
            textField.AddToClassList("dns-node__choice-textfield");
            textField.AddToClassList("dns-node__filename-textfield");
            textField.AddToClassList("dns-node__textfield__hidden");

            choicePort.Add(textField);
            choicePort.Add(deleteChoice);
            return choicePort;
        }
    }
}