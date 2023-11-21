using System;
using System.Collections.Generic;
using DecisionNS.Enumerations;
using UnityEngine;

namespace DecisionNS.Data.Save
{
    [System.Serializable]
    public class DNode
    {
        [field:SerializeField] public Int64 Id { get; set; }
        [field:SerializeField] public List<DNSChoiceSaveData> Choices { get; set; }
        [field:SerializeField] public string Text { get; set; }
        [field:SerializeField] public DNSTypes Type { get; set; }
        [field:SerializeField] public Vector2 Position { get; set; }

        public void Fill(Int64 Id, DNSTypes Type, string Text, Vector2 Position)
        {
            this.Id = Id;
            this.Type = Type;
            this.Text = Text;
            this.Position = Position;
        }
    }

    [System.Serializable]
    public class SingleDNode : DNode
    {
        [field:SerializeField] public bool IsNPC { get; set; }
        [field:SerializeField] public ScriptableObject NodeItem { get; set; }

        public void Fill(bool IsNPC, ScriptableObject NodeItem)
        {
            this.IsNPC = IsNPC;
            this.NodeItem = NodeItem;
        }
    }
}