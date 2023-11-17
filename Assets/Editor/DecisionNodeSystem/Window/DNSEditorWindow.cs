using DecisionNS.Utilities;
using UnityEditor;
using UnityEngine.UIElements;

namespace DecisionNS.Windows
{
    public class DNSEditorWindow : EditorWindow
    {
        [MenuItem("Window/DecisionNS/Decision Graph")]
        public static void ShowExample()
        {
            GetWindow<DNSEditorWindow>("Decision Graph");
        }

        private void OnEnable()
        {
            AddGraphView();

            AddStyle();
        }

        private void AddGraphView()
        {
            DNSGraphView dnsGraphView = new DNSGraphView(this);
            
            dnsGraphView.StretchToParentSize();
            
            rootVisualElement.Add(dnsGraphView);
        }

        private void AddStyle()
        {
            rootVisualElement.AddStyleSheets(
                "DecisionNodeSystem/DNSVariables.uss",
                "DecisionNodeSystem/DNSNodeStyle.uss");
        }
    }   
}