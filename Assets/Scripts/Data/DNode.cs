using System;
using System.Collections.Generic;
using DecisionNS.Editor.DecisionNodeSystem.Data.ScriptableObjects;
using DecisionNS.Enums;
using UnityEngine;

namespace DecisionNS.Data
{
    [System.Serializable]
    public class DNode
    {
        [field:SerializeField] public Int64 Id { get; set; }
        [field:SerializeField] public List<DNSPortLink> Port { get; set; }
        [field:SerializeField] public DNSTypes Type { get; set; }
        [field:SerializeField] public Vector2 Position { get; set; }

        public void Fill(Int64 Id, DNSTypes Type, Vector2 Position)
        {
            this.Id = Id;
            this.Type = Type;
            this.Position = Position;
        }
    }

    [System.Serializable]
    public class SingleDNode : DNode
    {
        [field:SerializeField] public bool IsNPC { get; set; }
        [field:SerializeField] public ScriptableObject NodeItem { get; set; }
        [field:SerializeField] public string Text { get; set; }

        public void Fill(bool IsNPC, ScriptableObject NodeItem, string Text)
        {
            this.IsNPC = IsNPC;
            this.NodeItem = NodeItem;
            this.Text = Text;
        }
    }
}