using System;
using UnityEngine;

namespace CodeGraph
{
    public class NodeinfoAttribute : Attribute
    {
        private string m_nodeTitle;
        private string m_menuItem;

        public string title => m_nodeTitle;
        public string menuItem => m_menuItem;

        public NodeinfoAttribute(string title, string menuItem = "")
        {
            m_nodeTitle = title;
            m_menuItem = menuItem;
        }
    }
}
