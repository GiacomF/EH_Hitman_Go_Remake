using System;
using UnityEditor.Experimental.GraphView;
using System.Reflection;
using UnityEngine;

namespace CodeGraph.Editor
{
    public class CodeGraphEditorNode : Node
    {
        private CodeGraphNode m_graphNode;
        public CodeGraphEditorNode(CodeGraphNode node)
        {
            this.AddToClassList("code-graph-node");
            
            m_graphNode = node;

            Type typeInfo = node.GetType();
            NodeinfoAttribute info = typeInfo.GetCustomAttribute<NodeinfoAttribute>();

            title = info.title;

            string[] depths = info.menuItem.Split('/');
            foreach (string depth in depths)
            {
                this.AddToClassList(depth.ToLower().Replace(' ', '-'));
            }
            
            this.name = typeInfo.Name;
        }
    }
}
