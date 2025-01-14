using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CodeGraph.Editor
{
    public class CodeGraphEditorNode : Node
    {
        public CodeGraphEditorNode()
        {
            this.AddToClassList("code-graph-node");
        }
    }
}
