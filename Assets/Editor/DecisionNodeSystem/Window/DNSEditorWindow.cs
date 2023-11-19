using System.Collections.Generic;
using System.Net;
using DecisionNS.Elements;
using DecisionNS.Utilities;
using NUnit.Framework;
using PlasticGui;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DecisionNS.Windows
{
    public class DNSEditorWindow : EditorWindow
    {
        private readonly string DefaultFileName = "DecisionFileName";
        private DNSGraphView graphView;

        private TextField fileNameTextField;
        
        [MenuItem("Window/DecisionNS/Decision Graph")]
        public static void ShowExample()
        {
            GetWindow<DNSEditorWindow>("Decision Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();
            
            AddStyle();
        }

        private void AddGraphView()
        {
            graphView = new DNSGraphView(this);
            
            graphView.StretchToParentSize();
            
            rootVisualElement.Add(graphView);
        }
        
        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            fileNameTextField = new TextField().CreateTextField(DefaultFileName);

            toolbar.Add(fileNameTextField);

            AddSaveButton(toolbar);
            AddLoadButton(toolbar);
            
            rootVisualElement.Add(toolbar);
        }

        private void AddLoadButton(Toolbar toolbar)
        {
            Button loadButton = new Button(() => LoadGraph())
            {
                text = "Load"
            };
            toolbar.Add(loadButton);
        }

        private void AddSaveButton(Toolbar toolbar)
        {
            Button saveButton = new Button((() =>
            {
                SaveGraph();
            }))
            {
                text = "Save"
            };

            graphView.onValueIndexError += error =>
            {
                if (error == 0)
                {
                    saveButton.SetEnabled(true);
                }
                else
                {
                    saveButton.SetEnabled(false);
                }
            };
            toolbar.Add(saveButton);
        }
        
        private List<DNSNode> nodes;

        private void SaveGraph()
        {
            if (string.IsNullOrEmpty(fileNameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name you've typed in is valid.", "Fine!");
                return;
            }

            nodes = new List<DNSNode>();

            graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is DNSNode node)
                {
                    nodes.Add(node);
                }
            });

            GraphData graphData = new GraphData(nodes);
            string json = JsonUtility.ToJson(graphData);
            Debug.Log(json);
            Load(json);
        }
        
        private void Load(string json)
        {
            GraphData graphData = JsonUtility.FromJson<GraphData>(json);   
            List<DNSNode> nodes2 = graphData.Nodes;

            foreach (var node in nodes2)
            {
                node.Log();
            }
        }
        
        private void LoadGraph()
        {
            string filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/Editor/DialogueSystem/Graphs", "asset");

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }
            // note: we need to clear graph before load anything.
            graphView.Clear();

            // todo load data in graph 
            // DSIOUtility.Initialize(graphView, Path.GetFileNameWithoutExtension(filePath));
            // DSIOUtility.Load();
        }

        private void AddStyle()
        {
            rootVisualElement.AddStyleSheets(
                "DecisionNodeSystem/DNSVariables.uss",
                "DecisionNodeSystem/DNSNodeStyle.uss");
        }
    }
    
    [System.Serializable]
    public class GraphData
    {
        [SerializeField]
        [SerializeReference]
        private List<DNSNode> nodes;

        public GraphData(List<DNSNode> nodes)
        {
            this.nodes = nodes;
        }

        public List<DNSNode> Nodes => nodes;
    }
}