using UnityEditor.Experimental.GraphView;

namespace DecisionNS.Editor.Memento
{
    public static class MementoGraph
    {
        private static GraphView graphView;

        public static void Save()
        {

            graphView.graphElements.ForEach(graphElement =>
            {
                // if (graphElement is DSNode node)
                // {
                //     nodes.Add(node);
                //
                //     return;
                // }
            });
        }
    }
}