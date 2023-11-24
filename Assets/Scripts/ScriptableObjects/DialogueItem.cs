using UnityEngine;

namespace DecisionNS.Editor.DecisionNodeSystem.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "dialogue_item", menuName = "DNS/Items/Dialogue Item", order = 0)]
    public class DialogueItem : BaseConfig
    {
        public string text;
    }
}