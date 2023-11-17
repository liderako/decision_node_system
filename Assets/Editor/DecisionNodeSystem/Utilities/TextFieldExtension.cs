using UnityEngine.UIElements;

namespace DecisionNS.Utilities
{
    public static class TextFieldExtension
    {
        public static TextField CreateTextField(this TextField textField, string value=null,
            EventCallback<ChangeEvent<string>> onValueChange=null)
        {
            textField = new TextField()
            {
                value = value
            };

            if (onValueChange != null)
            {
                textField.RegisterValueChangedCallback(onValueChange);
            }

            return textField;
        }

        public static TextField CreateTextArea(this TextField textField, string value=null,
            EventCallback<ChangeEvent<string>> onValueChange=null)
        {
            textField.CreateTextField(value, onValueChange);
            textField.multiline = true;
            return textField;
        }
    }
}