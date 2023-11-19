using DecisionNS.Data.Save;
using DecisionNS.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DecisionNS.Elements
{
    using Enumerations;
    
    [System.Serializable]
    public class DNSSingleChoiceNode : DNSNode
    {
        public bool IsNps = true;
        public override void Initialize(int id, Vector2 position, DNSGraphView graph)
        {
            base.Initialize(id, position, graph);

            DecisionName = "Single Decision Node";
            
            Type = DNSTypes.SingleChoice;
            
            Choices.Add(new DNSChoiceSaveData() { Text = "Output" });
        }

        public override void Draw()
        {
            base.Draw(); 

            // note: output container
            foreach (var choice in Choices)
            {
                outputContainer.Add(this.CreateOutput(Port.Capacity.Single, name:choice.Text));
            }
            
            RefreshExpandedState();
        }
        
        public override void Log()
        {
            base.Log();
            Debug.Log("IsNPC: " + IsNps);
        }
    }
}