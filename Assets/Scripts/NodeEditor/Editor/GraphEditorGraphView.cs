using System;
using System.Collections.Generic;
using System.Linq;
using NodeEditor.Graphs;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = NodeEditor.Nodes.Node;

namespace NodeEditor.Editor
{
    public class GraphEditorGraphView : GraphView
    {
        private Graph m_CurrentGraph = null;

        /// <summary>
        /// Constructor needed for GraphView
        /// </summary>
        public GraphEditorGraphView()
        {
            InitializeBackground();
            InitializeManipulators();
            LoadAndApplyStyleSheet("Assets/Scripts/NodeEditor/Editor/GraphEditor.uss");
        }

        private void InitializeBackground()
        {
            Insert(0, new GridBackground());
        }

        private void InitializeManipulators()
        {
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        private void LoadAndApplyStyleSheet(string path)
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path);
            if (styleSheet != null)
            {
                styleSheets.Add(styleSheet);
            }
            else
            {
                Debug.LogError("Failed to load StyleSheet: " + path);
            }
        }


        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList()
                .Where(endPort => IsPortCompatible(startPort, endPort))
                .ToList();
        }

        private bool IsPortCompatible(Port startPort, Port endPort)
        {
            // Check if directions are opposite
            if (endPort.direction == startPort.direction)
                return false;

            // Check if they belong to different nodes
            if (endPort.node == startPort.node)
                return false;

            // Type compatibility check (assuming a 'Type' property or similar)
            // if (startPort.Type != endPort.Type)
            //     return false;

            return true;
        }


        public void PopulateView(Graph graph)
        {
            m_CurrentGraph = graph;

            // Unsub has to go right before DeleteElements call (i.e. previous view is being cleaned)
            // Else the OnGraphViewChanged is called even when switching graph to be shown
            // leading to the total loss of the stored Graph
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            graph.Nodes.ForEach(n => CreateNodeView(n));

            foreach (var edge in graph.Edges)
            {
                var parentView = FindNodeView(edge.From);
                var childView = FindNodeView(edge.To);

                var edgeView = parentView.ConnectChild(childView);
                // TODO: it would be really nice to assign this directly into EdgeView
                edgeView.Data = edge;

                AddElement(edgeView);
            }
        }

        private NodeView FindNodeView(Node node)
        {
            if (GetNodeByGuid(node.Guid) is NodeView nodeView)
            {
                return nodeView;
            }

            // TODO: Handle the error better
            return null;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphviewchange)
        {
            if (graphviewchange.elementsToRemove != null)
            {
                foreach (var graphElement in graphviewchange.elementsToRemove)
                {
                    if (graphElement is NodeView nodeView)
                    {
                        DeleteNodeView(nodeView);
                    }

                    if (graphElement is EdgeView edge)
                    {
                        DeleteEdge(edge);
                    }
                }
            }

            if (graphviewchange.edgesToCreate != null)
            {
                foreach (var edge in graphviewchange.edgesToCreate)
                {
                    // TODO: Just pass and edge
                    if (edge.output.node is NodeView parentNode && edge.input.node is NodeView childNode)
                    {
                        if (edge is EdgeView edgeView)
                        {
                            // TODO: it would be really nice to assign this directly into EdgeView
                            edgeView.Data = m_CurrentGraph.CreateEdge(parentNode.Data, childNode.Data);

                            // TODO: Consider also input edges
                            parentNode.Data.AddEdge(edgeView.Data);
                        }
                    }
                }
            }

            return graphviewchange;
        }

        private void CreateNodeView(Node node)
        {
            NodeView nodeView = new NodeView(node);
            AddElement(nodeView);
        }

        private void DeleteNodeView(NodeView nodeView)
        {
            m_CurrentGraph.DeleteNode(nodeView.Data);
        }

        private void DeleteEdge(EdgeView edge)
        {
            edge.Data.From.RemoveEdge(edge.Data);
            m_CurrentGraph.DeleteEdge(edge.Data);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            var types = TypeCache.GetTypesDerivedFrom<Node>().ToList();
            types.Add(typeof(Node));

            foreach (var type in types.Where(t => t.IsAbstract == false))
            {
                evt.menu.AppendAction($"[{nameof(Node)}] {type}", (a) => CreateNode(type));
            }

            void CreateNode(Type nodeType)
            {
                CreateNodeView(m_CurrentGraph.CreateNode(nodeType));
            }
        }

        /// <summary>
        /// Nested class needed for GraphView
        /// </summary>
        public new class UxmlFactory : UxmlFactory<GraphEditorGraphView, GraphView.UxmlTraits>
        {
        }
    }
}