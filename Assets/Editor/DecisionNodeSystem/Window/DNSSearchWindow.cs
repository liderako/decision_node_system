using System.Collections.Generic;
using DecisionNS.Enumerations;
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
                new SearchTreeEntry(new GUIContent("Single Decision", indentationIcon))
                {
                    level = 2,
                    userData = DNSTypes.SingleChoice
                },
                new SearchTreeEntry(new GUIContent("Multiple Decision", indentationIcon))
                {
                    level = 2,
                    userData = DNSTypes.MultipleChoice
                },
                new SearchTreeGroupEntry(new GUIContent("Create Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", indentationIcon))
                {
                    level = 2,
                    userData = new Group()
                }
            };
                
            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);
            switch (SearchTreeEntry.userData)
            {
                case DNSTypes.SingleChoice:
                {
                    graphView.AddElement(graphView.CreateNode(
                        DNSTypes.SingleChoice,
                       position:localMousePosition));
                    return true;
                }
                case DNSTypes.MultipleChoice:
                {
                    graphView.AddElement(graphView.CreateNode(
                        DNSTypes.MultipleChoice,
                        position:localMousePosition));
                    return true;
                }
            }
            return false;
        }
    }
}