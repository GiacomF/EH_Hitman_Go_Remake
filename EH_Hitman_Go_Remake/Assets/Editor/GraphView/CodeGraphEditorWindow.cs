using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace CodeGraph.Editor
{
    public class CodeGraphEditorWindow : EditorWindow
    {
        public void DrawGraph()
        {
            m_serializedObject = new SerializedObject(m_currentGraph);
            m_currentView = new CodeGraphView(m_serializedObject, this);
            rootVisualElement.Add(m_currentView);
        }

        public void Load(CodeGraphAsset target)
        {
            m_currentGraph = target;
            DrawGraph();
        }

        [SerializeField]
        private CodeGraphAsset m_currentGraph;
        [SerializeField]
        private SerializedObject m_serializedObject;
        [SerializeField]
        private CodeGraphView m_currentView;
        public CodeGraphAsset currentGraph => m_currentGraph;
        public static void Open(CodeGraphAsset target)
        {
            CodeGraphEditorWindow[] windows = Resources.FindObjectsOfTypeAll<CodeGraphEditorWindow>();
            foreach (CodeGraphEditorWindow w in windows)
            {
                if(w.currentGraph == target)
                {
                    w.Focus();
                    return;
                }
            }

            CodeGraphEditorWindow window = CreateWindow<CodeGraphEditorWindow>(typeof(CodeGraphEditorWindow), typeof(SceneView));
            window.titleContent = new GUIContent($"{target.name}", EditorGUIUtility.ObjectContent(null, typeof(CodeGraphAsset)).image);
            window.Load(target);
        }

    }
}
