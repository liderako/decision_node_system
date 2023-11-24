using System.IO;
using DecisionNS.Editor.DecisionNodeSystem.Data.ScriptableObjects;
using DecisionNS.Editor.Memento;
using DecisionNS.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using DecisionNS.Data;

namespace DecisionNS.Windows
{
    public class DNSEditorWindow : EditorWindow
    {
        private readonly string DefaultFileName = "DecisionFileName";
        private DNSGraphView graphView;
        private MementoGraph mementoGraph;

        private TextField fileNameTextField;

        public DNSContainer dnsContainer;
        public DNSContainer originContainer;

        [MenuItem("Window/DecisionNS/Decision Graph")]
        public static void ShowExample()
        {
            GetWindow<DNSEditorWindow>("Decision Graph");
        }

        public static void OpenWindow(DNSContainer dnsContainerInput)
        {
            var w = GetWindow<DNSEditorWindow>("Decision Graph");
            var copyContainer = Instantiate(dnsContainerInput);
            copyContainer.name = dnsContainerInput.name;
            w.LoadGraph(copyContainer);
            w.dnsContainer = copyContainer;
            w.originContainer = dnsContainerInput;
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();
            AddStyle();
            mementoGraph = new MementoGraph(graphView, DefaultFileName);
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
            AddResetButton(toolbar);
            
            rootVisualElement.Add(toolbar);
        }

        private void AddResetButton(Toolbar toolbar)
        {
            Button reset = new Button(() =>
            {
                if (dnsContainer != null)
                {
                    LoadGraph(dnsContainer);
                }
            })
            {
                text = "Reset"
            };
            toolbar.Add(reset);
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
            if (originContainer == null)
            {
                if (string.IsNullOrEmpty(fileNameTextField.value))
                {
                    EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name you've typed in is valid.", "Fine!");
                    return;
                }
                mementoGraph.Save(fileNameTextField.value, graphView.GetNodesForSave());
            }
            else
            {
                mementoGraph.Save(fileNameTextField.value, graphView.GetNodesForSave(), originContainer);   
            }
        }

        private void LoadGraph(DNSContainer container=null)
        {
            string fileName;
            if (container == null)
            {
                string filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/", "asset");

                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }
                fileName = Path.GetFileNameWithoutExtension(filePath);
            }
            else
            {
                fileName = container.name;
            }
            graphView.ClearGraph();
            mementoGraph = new MementoGraph(graphView, fileName);
            if (mementoGraph.Load(container))
            {
                fileNameTextField.value = fileName;
            }
        }

        private void AddStyle()
        {
            rootVisualElement.AddStyleSheets(
                "DecisionNodeSystem/DNSVariables.uss",
                "DecisionNodeSystem/DNSNodeStyle.uss");
        }
    }
}