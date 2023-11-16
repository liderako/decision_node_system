using System;
using DecisionNS.Elements;
using DecisionNS.Enumerations;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DecisionNS
{
    public class DNSGraphView : GraphView
    {
        public DNSGraphView()
        {
            AddManipulators();
            AddGridBackground();

            AddStyle();
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(CreateNodeContextMenu( "Add Node (Single Choice)", DNSTypes.SingleChoice));
            this.AddManipulator(CreateNodeContextMenu("Add Node (Multiple Choice)" , DNSTypes.MultipleChoice));
            this.AddManipulator(new ContentDragger());
        }

        private IManipulator CreateNodeContextMenu(string actionTitle, DNSTypes type)
        {
            ContextualMenuManipulator menuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, 
                    actionEvent => AddElement(CreateNode(type, actionEvent.eventInfo.localMousePosition))));
            return menuManipulator;
        }
        
        private DNSNode CreateNode(DNSTypes decisionType, Vector2 position)
        {
            Type type = Type.GetType($"DecisionNS.Elements.DNS{decisionType}Node");

            DNSNode node = (DNSNode)Activator.CreateInstance(type);
            node.Initialize(position);
            node.Draw();
            return node;
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        private void AddStyle()
        {
            StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load("DecisionNodeSystem/DNSGraphViewStyle.uss");
            styleSheets.Add(styleSheet);
        }
    }
}
