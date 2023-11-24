using System;
using System.Collections.Generic;
using DecisionNS.Data;
using UnityEngine;

namespace DecisionNS.Editor.DecisionNodeSystem.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "dns_container", menuName = "DNS/Container", order = 0)]
    public class DNSContainer : ScriptableObject
    {
        [SerializeReference] public List<DNode> Nodes;
    }
    
    [Serializable]
    public class DNSPortLink
    {
        [field: SerializeField] public long NodeID { get; set; }
    }
}