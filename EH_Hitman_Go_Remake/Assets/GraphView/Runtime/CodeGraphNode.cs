using System;
using UnityEngine;

namespace CodeGraph
{
    [System.Serializable]
    public class CodeGraphNode : MonoBehaviour
    {
        [SerializeField]
        private string m_guid;
        [SerializeField]
        private Rect m_position;

        public string typeName;
        public string id => m_guid;
        public Rect position => m_position;

        public CodeGraphNode()
        {
            m_guid = Guid.NewGuid().ToString();
        }

        public void SetPosition(Rect position)
        {
            m_position = position;
        }
    }
}
