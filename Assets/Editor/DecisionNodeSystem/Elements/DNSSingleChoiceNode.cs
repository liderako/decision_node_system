using DecisionNS.Utilities;
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

            DecisionName = "Single Decision Node";
            
            Type = DNSTypes.SingleChoice;
            
            Choices.Add("Output");
        }

        public override void Draw()
        {
            base.Draw(); 

            // note: output container
            foreach (var choice in Choices)
            {
                outputContainer.Add(this.CreateOutput(Port.Capacity.Single, name:choice));
            }
            
            RefreshExpandedState();
        }
    }
}