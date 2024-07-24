using System.Collections.Generic;
using System.Linq;
using NodeEditor.Edges;
using NodeEditor.Nodes;
using UnityEngine;
using UnityEngine.Serialization;

namespace NodeEditor.Graphs
{
    [CreateAssetMenu(menuName = "Editor/GraphEditor/Graph")]
    public partial class Graph : ScriptableObject
    {
        [SerializeField] private bool b_Persist = false;
        
        [SerializeField] private bool b_IsFlowGraph = false;
        
        [SerializeField] private Node m_Root;
        
        // TODO: Hide in inspector to ensure data consistency
        [SerializeField, HideInInspector] private List<Node> m_Nodes = new List<Node>(0);
        [FormerlySerializedAs("_edges")] [SerializeField, HideInInspector] private List<Edge> m_Edges = new List<Edge>(0);

        public List<Node> Nodes => m_Nodes;
        public List<Edge> Edges => m_Edges;

        public GraphInstance GetInstance(MonoBehaviour owner) => new GraphInstance(m_Root, owner, this);
        
        private Node FindNodeByGuid(string guid)
        {
            return m_Nodes.FirstOrDefault(node => node.Guid == guid);
        }
    }
}