using System;
using NodeEditor.Edges;
using NodeEditor.Nodes;
using UnityEditor;
using UnityEngine;

namespace NodeEditor.Graphs
{
    public partial class Graph
    {
#if UNITY_EDITOR
        public Node CreateNode(Type type)
        {
            if ((type.IsSubclassOf(typeof(Node)) || type == typeof(Node)) && type.IsAbstract == false)
            {
                Node node = (Node)ScriptableObject.CreateInstance(type);

                node.name = $"{node.GetType().Name} - {node.Guid}";

                m_Nodes.Add(node);

                AssetDatabase.AddObjectToAsset(node, this);
                AssetDatabase.SaveAssets();

                return node;
            }

            throw new InvalidOperationException("Type must be a subclass of Node");
        }

        public void DeleteNode(Node node)
        {
            m_Nodes.Remove(node);

            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        public Edge CreateEdge(Node from, Node to)
        {
            Edge edge = Edge.GetInstance(from, to);

            m_Edges.Add(edge);

            AssetDatabase.AddObjectToAsset(edge, this);
            AssetDatabase.SaveAssets();

            return edge;
        }

        public void DeleteEdge(Edge edge)
        {
            m_Edges.Remove(edge);

            AssetDatabase.RemoveObjectFromAsset(edge);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}