using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace CodeGraph
{
    [CreateAssetMenu(menuName = "CodeGraph/NewGraph")]
    public class CodeGraphAsset : ScriptableObject
    {
        [SerializeReference]
        private List<CodeGraphNode> m_nodes;

        public List<CodeGraphNode> Nodes => m_nodes;

        public CodeGraphAsset()
        {
            m_nodes = new List<CodeGraphNode>();
        }
    }
}
