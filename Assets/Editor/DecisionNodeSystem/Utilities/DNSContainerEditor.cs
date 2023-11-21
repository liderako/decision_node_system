using DecisionNS.Editor.DecisionNodeSystem.Data.ScriptableObjects;
using DecisionNS.Windows;
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEngine;

namespace DecisionNS.Utilities
{
    public class DNSContainerEditor : UnityEditor.Editor
    {
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Object obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj is DNSContainer)
            {
                DNSEditorWindow.OpenWindow((DNSContainer)obj);
                return true;
            }
            return false;
        }
    }
}