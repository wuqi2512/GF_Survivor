using GameFramework;
using System;
using System.Collections.Generic;

public partial class RedDotComponent
{
    public class Node : IReference
    {
        private string m_Name;
        private int m_Value;
        private Node m_Parent;
        private List<Node> m_Childs;
        private Action<int> m_OnValueChanged;

        public static Node Create(string name, Node parent)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Name of node is invalid.");
            }

            Node node = ReferencePool.Acquire<Node>();
            node.m_Name = name;
            node.m_Parent = parent;

            return node;
        }

        public string Name => m_Name;
        public int Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                if (m_Value == value)
                {
                    return;
                }

                m_Value = value;
                GameEntry.RedDot.SetNodeDirty(Parent);
                m_OnValueChanged?.Invoke(m_Value);
            }
        }
        public Node Parent => m_Parent;
        public event Action<int> OnValueChanged
        {
            add => m_OnValueChanged += value;
            remove => m_OnValueChanged -= value;
        }

        public void RefreashValue()
        {
            if (m_Childs == null)
            {
                return;
            }

            int newValue = 0;
            foreach (Node child in m_Childs)
            {
                newValue += child.Value;
            }

            Value = newValue;
        }

        public void Clear()
        {
            m_Name = null;
            m_Value = 0;
            m_Parent = null;
            foreach (Node child in m_Childs)
            {
                ReferencePool.Release(child);
            }
            m_Childs.Clear();
        }

        public Node GetNode(string name)
        {
            if (m_Childs == null)
            {
                return null;
            }

            foreach (Node child in m_Childs)
            {
                if (child.Name == name)
                {
                    return child;
                }
            }

            return null;
        }

        public Node GetOrAddChild(string name)
        {
            Node node = GetNode(name);
            if (node != null)
            {
                return node;
            }

            node = Create(name, this);

            if (m_Childs == null)
            {
                m_Childs = new List<Node>();
            }
            m_Childs.Add(node);

            return node;
        }

        public void RemoveChild(string name)
        {
            Node node = GetNode(name);
            if (node == null)
            {
                return;
            }

            m_Childs.Remove(node);
            ReferencePool.Release(node);
        }
    }
}