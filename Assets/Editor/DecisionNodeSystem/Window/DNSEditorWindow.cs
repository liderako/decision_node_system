using DecisionNS.Utilities;
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

        private void SaveGraph()
        {
            if (string.IsNullOrEmpty(fileNameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name you've typed in is valid.", "Fine!");
                return;
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
}