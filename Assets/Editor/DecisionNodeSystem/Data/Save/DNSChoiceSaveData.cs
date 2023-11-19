using System;
using UnityEngine;

namespace DecisionNS.Data.Save
{
    [Serializable]
    public class DNSChoiceSaveData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public int NodeID { get; set; }
    }
}