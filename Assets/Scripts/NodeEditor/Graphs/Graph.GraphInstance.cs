using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NodeEditor.Nodes;
using UnityEngine;

namespace NodeEditor.Graphs
{
    public partial class Graph
    {
        public class GraphInstance
        {
            public static event Action OnNodeUpdated = null;
            
            private const string graphSerializationKey = "visitedNodes";
            
            private readonly Node m_Root;

            private readonly MonoBehaviour m_Owner;

            private Coroutine m_Running;

            public bool IsDone => m_Running == null;

            private Stack<Node> m_NodesTraversed = new Stack<Node>(0);

            private bool b_Persist = false;

            private bool b_IsFlowGraph = false;
            
            public GraphInstance(Node root, MonoBehaviour owner, Graph graph)
            {
                m_Root = root;
                m_Owner = owner;
                b_Persist = graph.b_Persist;
                b_IsFlowGraph = graph.b_IsFlowGraph;
                
                RetrieveVisitedNodesFromSuspend(ScormManager.Instance.GetCustomList(graphSerializationKey), graph);
                
                foreach (var node in m_NodesTraversed)
                {
                    if (node != m_NodesTraversed.Peek())
                    {
                        node.OnRestore();
                    }
                }
            }

            public void Run()
            {
                if (m_Running != null)
                {
                    return;
                }

                m_Running = m_Owner.StartCoroutine(LifeCycle());
            }

            public void Interrupt()
            {
                if (m_Running != null)
                {
                    m_Owner.StopCoroutine(m_Running);
                    m_Running = null;
                }
                
                m_NodesTraversed.Pop().OnRevert();
                
                m_NodesTraversed.Clear();
                
                TrySaveState();
            }

            private void Stop()
            {
                if (m_Running == null)
                {
                    return;
                }

                m_Owner.StopCoroutine(m_Running);
                m_Running = null;
            }

            public void Back()
            {
                if (m_NodesTraversed.Count == 1)
                {
                    return;
                }

                Stop();

                m_NodesTraversed.Pop().OnRevert();
                
                Run();
            }

            private IEnumerator LifeCycle()
            {
                Node current = null;
                
                if (m_NodesTraversed.Count == 0)
                {
                    current = m_Root;
                    m_NodesTraversed.Push(current);
                    TrySaveState();

                    if (b_IsFlowGraph)
                    {
                        OnNodeUpdated?.Invoke();
                    }
                }
                else
                {
                    current = m_NodesTraversed.Peek();

                    if (b_IsFlowGraph)
                    {
                        OnNodeUpdated?.Invoke();
                    }
                }
                
                current.OnEnter();

                yield return new WaitForSeconds(0.00001f);

                while (current != null)
                {
                    current = current.GetNext();

                    if (current != m_NodesTraversed.Peek())
                    {
                        m_NodesTraversed.Peek().OnExit();
                        
                        if (current != null)
                        {
                            current.OnEnter();
                            m_NodesTraversed.Push(current);
                            TrySaveState();

                            if (b_IsFlowGraph)
                            {
                                OnNodeUpdated?.Invoke();   
                            }
                        }
                    }
                    else
                    {
                        current.OnStay();
                    }

                    yield return new WaitForSeconds(0.00001f);
                }
                
                Stop();
            }
            
            private List<string> VisitedNodeGuids => m_NodesTraversed.Select(node => node.Guid).ToList();

            private void TrySaveState()
            {
                if (b_Persist)
                {
                    ScormManager.Instance.StoreCustomData(graphSerializationKey,VisitedNodeGuids);
                }
            }

            private void RetrieveVisitedNodesFromSuspend(List<string> visitedNodes, Graph graph)
            {
                m_NodesTraversed.Clear();
                
                visitedNodes.Reverse();
                
                foreach (var guid in visitedNodes)
                {
                    var node = graph.FindNodeByGuid(guid);
                    if (node != null)
                    {
                        m_NodesTraversed.Push(node);
                    }
                }
            }

            public int GetCurrentSteps() => m_NodesTraversed.Count;
            
            public int GetMissingSteps()
            {
                Node current = m_NodesTraversed.Count > 0 ? m_NodesTraversed.Peek() : m_Root;

                return current.GetSteps();
            }
        }
    }
}