using System.Collections.Generic;
using System.Linq;
using CareBoo.Serially;
using NodeEditor.Actions;
using NodeEditor.Edges;
using UnityEngine;

namespace NodeEditor.Nodes
{
    public partial class Node : GraphElement
    {
        [SerializeField] private List<Edge> m_Edges = new List<Edge>(0);

        [SerializeReference, ShowSerializeReference, Tooltip("Called when entering a node. It's always called")]
        private List<Action> m_OnEnterActions = new List<Action>(0);

        [SerializeReference, ShowSerializeReference,
         Tooltip("Called when staying into a node. It might not be called at all.")]
        private List<Action> m_OnStayActions = new List<Action>(0);

        [SerializeReference, ShowSerializeReference, Tooltip("Called when exiting a node. It's always called")]
        private List<Action> m_OnExitActions = new List<Action>(0);
        
        [SerializeReference, ShowSerializeReference,
         Tooltip("Called when reverting a node. Called when the back button is pressed")]
        private List<Action> m_OnRevertActions = new List<Action>(0);

        [SerializeReference, ShowSerializeReference,
         Tooltip(
             "Called when restoring a node. Called when a session is resumed and all nodes are traversed to reach the saved one")]
        private List<Action> m_OnRestoreActions = new List<Action>(0);

        public virtual void OnEnter()
        {
            foreach (var action in m_OnEnterActions)
            {
                action.Execute();
            }
        }

        public virtual void OnStay()
        {
            foreach (var action in m_OnStayActions)
            {
                action.Execute();
            }
        }

        public virtual void OnExit()
        {
            foreach (var action in m_OnExitActions)
            {
                action.Execute();
            }
        }
        
        public virtual void OnRevert()
        {
            foreach (var action in m_OnRevertActions)
            {
                action.Execute();
            }
        }
        
        public virtual void OnRestore()
        {
            foreach (var action in m_OnRestoreActions)
            {
                action.Execute();
            }
        }


        /// <summary>
        /// Retrieves the next Node in the graph based on the evaluation of its edges.
        /// Iterates through each edge and evaluates its conditions to determine the next node.
        /// If no suitable edge is found, returns null if the current node is an end node, otherwise returns the current node.
        /// </summary>
        /// <returns>
        /// The next Node object if a suitable edge condition is met, null if the current node is an end node, 
        /// or the current node itself if no suitable edge is found and it is not an end node.
        /// </returns>
        public virtual Node GetNext()
        {
            foreach (var edge in m_Edges)
            {
                Node next = edge.Evaluate();

                if (next != null)
                {
                    return next;
                }
            }

            return IsEndNode() ? null : this;
        }

        public int GetSteps(int acc = 0)
        {
            if (IsEndNode())
            {
                return acc;
            }

            int min = int.MaxValue;
            
            foreach (var edge in m_Edges)
            {
                int steps = edge.To.GetSteps(acc + 1);

                min = Mathf.Min(steps, min);
            }

            return min;
        }

        private bool IsEndNode() => m_Edges.Count == 0;
    }
}