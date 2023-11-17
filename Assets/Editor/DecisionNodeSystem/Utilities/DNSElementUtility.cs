using UnityEditor;
using UnityEngine.UIElements;

namespace DecisionNS.Utilities
{

    public static class DNSElementUtility
    {
        public static VisualElement AddClass(this VisualElement element, params string[] classNames)
        {
            foreach (var n in classNames)
            {
                element.AddToClassList(n);   
            }
            return element;
        }

        public static VisualElement AddStyleSheets(this VisualElement element, params string[] styleSheetsName)
        {
            foreach (var title in styleSheetsName)
            {
                element.styleSheets.Add((StyleSheet) EditorGUIUtility.Load(title));
            }
            return element;
        }
    }
}