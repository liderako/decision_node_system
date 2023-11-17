using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DecisionNS.Elements
{
    using Enumerations;
    
    public class DNSSingleChoiceNode : DNSNode
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);

            Type = DNSTypes.SingleChoice;
            
            Choices.Add("Output");
        }

        public override void Draw()
        {
            base.Draw();

            // note: output container
            
            foreach (var choice in Choices)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single,
                    typeof(bool));

                choicePort.portName = choice;
                
                outputContainer.Add(choicePort);
            }
            
            RefreshExpandedState();
        }
    }
}