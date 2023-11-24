using System.Collections.Generic;
using System.Linq;
using DecisionNS.Data;
using DecisionNS.Editor.DecisionNodeSystem.Data.ScriptableObjects;
using DecisionNS.Elements;
using DecisionNS.Windows;
using Dythervin.Collections;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DecisionNS.Editor.Memento
{

    public class MementoGraph
    {
        private DNSGraphView graphView;
        private string graphFileName;
        private DNSEditorWindow dnsEditorWindow;

        public MementoGraph(DNSGraphView graphView, string graphName)
        {
            this.graphView = graphView;
            graphFileName = graphName;   
        }

        public void Save(string graphFileName, List<DNode> nodes, DNSContainer dnsContainer = null)
        {
            CreateDefaultFolders();
            if (dnsContainer == null)
            {
                DNSContainer graphData = CreateAsset<DNSContainer>("Assets/Resources/DecisionGraphs/", $"{graphFileName}");
                graphData.Nodes = nodes;
                SaveAsset(graphData);
            }
            else
            {
                dnsContainer.Nodes = nodes;
                SaveAsset(dnsContainer);
            }
        }
        
        private void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            T asset = LoadAsset<T>(path, assetName);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();

                AssetDatabase.CreateAsset(asset, fullPath);
            }

            return asset;
        }
        
        private T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        private void CreateDefaultFolders()
        {
            CreateFolder("Assets", "Resources");
            CreateFolder("Assets/Resources", "DecisionGraphs");
        }

        private void CreateFolder(string parentFolderPath, string newFolderName)
        {
            if (AssetDatabase.IsValidFolder($"{parentFolderPath}/{newFolderName}"))
            {
                return;
            }
            AssetDatabase.CreateFolder(parentFolderPath, newFolderName);
        }

        public bool Load(DNSContainer graphData=null)
        {
            if (graphData == null)
            {
                graphData = LoadAsset<DNSContainer>("Assets/Resources/DecisionGraphs/", $"{graphFileName}");

                if (graphData == null)
                {
                    EditorUtility.DisplayDialog(
                        "Could not find the file!",
                        "The file at the following path could not be found:\n\n" +
                        $"\"Assets/Resources/DecisionGraphs/{graphFileName}\".\n\n" +
                        "Make sure you chose the right file and it's placed at the folder path mentioned above.",
                        "Thanks!"
                    );
                    return false;
                }   
            }
            LoadNodes(graphData.Nodes);
            return true;
        }
        
        private  void LoadNodes(List<DNode> nodes)
        {
            var loadedNodes = new SerializedRefDictionary<long, (DNode, DNSNode)>();
            foreach (var nodeData in nodes)
            {
                DNSNode node = graphView.CreateNode(nodeData.Type, nodeData.Position, false);
                node.UploadSaveData(nodeData);
                node.Draw();
                graphView.AddElement(node);
                loadedNodes.Add(node.Id, (nodeData, node));
            }



            foreach (KeyValuePair<long, (DNode, DNSNode)> loadedNode in loadedNodes)
            {
                Port output = (Port) loadedNode.Value.Item2.outputContainer.Children().First();
                for (int i = 0; i < loadedNode.Value.Item1.Port.Count; i++)
                {
                    DNSNode nextNode = loadedNodes[loadedNode.Value.Item1.Port[i].NodeID].Item2;
                    Port input = (Port) nextNode.inputContainer.Children().First();
                    
                    Edge edge = output.ConnectTo(input);
                    graphView.AddElement(edge);
                    loadedNode.Value.Item2.RefreshPorts();
                }
            }
        }
    }
}