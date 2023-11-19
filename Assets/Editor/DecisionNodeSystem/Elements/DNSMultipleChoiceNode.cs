using DecisionNS.Data.Save;
using DecisionNS.Enumerations;
using DecisionNS.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DecisionNS.Elements
{
    public class DNSMultipleChoiceNode : DNSNode
    {
        private DNSGraphView dsGraphView;
        public override void Initialize(int id, Vector2 position, DNSGraphView dsGraphView)
        {
            base.Initialize(id, position, dsGraphView);

            DecisionName = "Multiple Decision Node";

            Type = DNSTypes.MultipleChoice;

            Choices.Add(new DNSChoiceSaveData() { Text = "New Choice" });

            this.GetHashCode();
        }

        public override void Draw()
        {
            base.Draw();
            
            // note: main container
            Button addChoice = new Button(() =>
            {
                var userdata = new DNSChoiceSaveData() { Text = "New Choice" };
                Choices.Add(userdata);
                Port port = CreatePort(userdata);
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

        private Port CreatePort(object userData)
        {
            Port choicePort = this.CreateOutput(Port.Capacity.Multi);
            choicePort.userData = userData;
            
            DNSChoiceSaveData choiceData = (DNSChoiceSaveData) userData;            
            
            Button deleteChoice = new Button((() =>
            {
                if (Choices.Count == 1)
                {
                    return;
                }
                if (choicePort.connected)
                {
                    graphView.DeleteElements(choicePort.connections);
                }
                Choices.Remove(choiceData);
                graphView.RemoveElement(choicePort);
            })).Setup(title:"Delete");
            
            deleteChoice.AddToClassList("dns_node__button");

            
            TextField textField = new TextField().CreateTextField(choiceData.Text, callback =>
            {
                choiceData.Text = callback.newValue;
            });
            
            textField.AddToClassList("dns-node__choice-textfield");
            textField.AddToClassList("dns-node__filename-textfield");
            textField.AddToClassList("dns-node__textfield__hidden");

            choicePort.Add(textField);
            choicePort.Add(deleteChoice);
            return choicePort;
        }
    }
}