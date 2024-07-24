using System;
using CareBoo.Serially;
using NodeEditor.Conditions;
using NodeEditor.Nodes;
using UnityEngine;

namespace NodeEditor.Edges
{
    // TODO: Show condition string in the inspector
    // TODO: Consider showing some tooltips
    public class Edge : GraphElement
    {
        [SerializeReference, ShowSerializeReference] private Condition m_Condition = null;
        
        [SerializeField] private Node m_From = null;
        [SerializeField] private Node m_To = null;
        
        public Node From => m_From;
        public Node To => m_To;

        public override string ToString() => m_Condition != null ? $"{m_Condition}" : string.Empty;

        public Node Evaluate() => m_Condition == null || m_Condition.Evaluate() == true ? m_To : null;

#if UNITY_EDITOR
        public System.Action onDataChanged;
        
        public static Edge GetInstance(Node from, Node to)
        {
            if (from == null || to == null)
            {
                throw new ArgumentNullException("Both 'from' and 'to' nodes must be provided.");
            }
            if (from == to)
            {
                throw new ArgumentException("An edge cannot connect a node to itself.");
            }

            Edge edge = CreateInstance<Edge>();
            edge.m_From = from;
            edge.m_To = to;
            edge.name = $"{edge.GetType().Name} - {from.name} to {to.name}";
            return edge;
        }
        
        private void OnValidate()
        {
            onDataChanged?.Invoke();
        }
#endif
    }
}