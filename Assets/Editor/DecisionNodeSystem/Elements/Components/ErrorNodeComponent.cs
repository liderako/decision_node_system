using DecisionNS.Data.Error;
using DecisionNS.Elements;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace DecisionNS.Editor.DecisionNodeSystem.Elements.Components
{
    public class ErrorNodeComponent
    {
        public bool IsError { get; private set; }
        private int indexError;

        private DNSNode owner;

        public ErrorNodeComponent(DNSNode node)
        {
            owner = node;
        }
        
        public void ActivateErrorNode()
        {
            indexError++;
            IsError = true;
            owner.SetErrorStyle(DNSErrorData.Color);
            owner.graphView.IncreaseIndexError();
        }

        public void DeactivateErrorNode()
        {
            indexError--;
            if (indexError <= 0)
            {
                indexError = 0;
                IsError = false;
                owner.ResetStyle();   
            }
            owner.graphView.DecreaseIndexError();
        }
        
        public void ResetAllErrors()
        {
            while (indexError > 0)
            {
                indexError--;
                owner.graphView.DecreaseIndexError();
            }
        }
        
        public void RegisterFieldCallBack(ObjectField field)
        {
            field.RegisterCallback<ChangeEvent<Object>>((evt) =>
            {
                if (evt.newValue is null && !IsError)
                {
                    ActivateErrorNode();
                }
                else if (evt.newValue is not null && IsError)
                {
                    DeactivateErrorNode();
                }
            });
        }
    }
}