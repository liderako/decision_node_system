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
            DNSGraphView dnsGraphView = new DNSGraphView();
            
            dnsGraphView.StretchToParentSize();
            
            rootVisualElement.Add(dnsGraphView);
        }

        private void AddStyle()
        {
            StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load("DecisionNodeSystem/DNSVariables.uss");
            rootVisualElement.styleSheets.Add(styleSheet);
        }
    }   
}