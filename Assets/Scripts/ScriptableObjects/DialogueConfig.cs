using UnityEngine;

namespace DecisionNS.Editor.DecisionNodeSystem.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "dialogue_item", menuName = "DNS/Items/DialogueItem", order = 0)]
    public class DialogueConfig : BaseConfig
    {
        public string text;
    }
}