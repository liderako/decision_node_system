using UnityEngine;

namespace DecisionNS.Editor.DecisionNodeSystem.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "condition_item", menuName = "DNS/Items/Condition Item", order = 2)]
    public class DNSConditionItem : BaseConfig
    {
        [field:SerializeField] public bool Status { get; set; }
    }
}