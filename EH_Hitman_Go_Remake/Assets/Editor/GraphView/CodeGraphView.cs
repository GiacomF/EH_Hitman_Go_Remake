using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeGraph.Editor
{
    public class CodeGraphView : GraphView
    {
        private CodeGraphAsset m_codeGraph;
        private SerializedObject m_serializedObject;
        private CodeGraphEditorWindow m_window;

        public List<CodeGraphEditorNode> m_graphNodes;
        public Dictionary<string, CodeGraphEditorNode> m_nodeDictionary;

        private CodeGraphWindowSearchProvider m_searchProvider;

        public CodeGraphEditorWindow window => m_window;

        public CodeGraphView(SerializedObject serializedObject, CodeGraphEditorWindow window)
        {
            m_serializedObject = serializedObject;
            m_codeGraph = (CodeGraphAsset)serializedObject.targetObject;
            m_window = window;

            m_graphNodes = new List<CodeGraphEditorNode>();
            m_nodeDictionary = new Dictionary<string, CodeGraphEditorNode>();
            m_searchProvider = ScriptableObject.CreateInstance<CodeGraphWindowSearchProvider>();
            m_searchProvider.graph = this;
            this.nodeCreationRequest = ShowSearchWindow;

            StyleSheet style = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/USS/CodeGraphEditor.uss");
            styleSheets.Add(style);

            GridBackground background =  new GridBackground();
            background.name = "Grid";
            Add(background);
            background.SendToBack();

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());
            this.AddManipulator(new ContentZoomer());
        }

        public void Add(CodeGraphNode node)
        {
            Undo.RecordObject(m_serializedObject.targetObject, "Added Node");
            m_codeGraph.Nodes.Add(node);
            m_serializedObject.Update();

            AddNodeToGraph(node);
        }

        private void AddNodeToGraph(CodeGraphNode node)
        {
            node.typeName = node.GetType().AssemblyQualifiedName;
            CodeGraphEditorNode editorNode = new CodeGraphEditorNode(node);
            editorNode.SetPosition(node.position);
            m_graphNodes.Add(editorNode);
            m_nodeDictionary.Add(node.id, editorNode);
            
            AddElement(editorNode);
        }

        private void ShowSearchWindow(NodeCreationContext ctx)
        {
            m_searchProvider.target = (VisualElement)focusController.focusedElement;
            SearchWindow.Open(new SearchWindowContext(ctx.screenMousePosition), m_searchProvider);
        }
    }
}