using System.Collections.Generic;
using DecisionNS.Enums;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DecisionNS.Windows
{
    public class DNSSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DNSGraphView graphView;
        private Texture2D indentationIcon;
        
        public void Initialize(DNSGraphView graphView)
        {
            this.graphView = graphView;
            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0,0, Color.clear);
            indentationIcon.Apply();
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
                new SearchTreeGroupEntry(new GUIContent("Decision Node"), 1),
                new SearchTreeEntry(new GUIContent("Decision Node", indentationIcon))
                {
                    level = 2,
                    userData = DNSTypes.Decision
                },
                new SearchTreeEntry(new GUIContent("Event Node", indentationIcon))
                {
                    level = 2,
                    userData = DNSTypes.Event
                },
                new SearchTreeEntry(new GUIContent("Condition Node", indentationIcon))
                {
                    level = 2,
                    userData = DNSTypes.Condition
                },
            };
                
            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);
            switch (SearchTreeEntry.userData)
            {
                case DNSTypes.Decision:
                {
                    graphView.AddElement(graphView.CreateNode(
                        DNSTypes.Decision,
                       position:localMousePosition));
                    return true;
                }
                case DNSTypes.Event:
                {
                    graphView.AddElement(graphView.CreateNode(
                        DNSTypes.Event,
                        position:localMousePosition));
                    return true;
                }
                case DNSTypes.Condition:
                {
                    graphView.AddElement(graphView.CreateNode(
                        DNSTypes.Condition,
                        position:localMousePosition));
                    return true;
                }
            }
            return false;
        }
    }
}