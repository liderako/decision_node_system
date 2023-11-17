using UnityEngine.UIElements;

namespace DecisionNS.Utilities
{
    public static class FoldoutExtension
    {
        public static Foldout CreateFoldout(this Foldout foldout, string title, bool collapsed = false)
        {
            foldout.text = title;
            foldout.value = !collapsed;
            
            return foldout;
        }
    }
}