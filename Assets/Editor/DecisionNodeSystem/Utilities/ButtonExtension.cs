using UnityEngine.UIElements;

namespace DecisionNS.Utilities
{
    public static class ButtonExtension
    {
        public static Button Setup(this Button button, string title)
        {
            button.text = title;
            return button;
        }
    }
}