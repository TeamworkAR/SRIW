#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

using System;
using UnityEngine;
using Edge = NodeEditor.Edges.Edge;

namespace NodeEditor.Nodes
{
    // TODO: Make Node return a NodeView (or a subclass) in order to handle different nodes view (e.g. State node with dynamic ports each representing a transition)
    public abstract partial class Node
    {
#if UNITY_EDITOR
        [SerializeField] private Vector2 _position = Vector2.zero;

        public Vector2 Position => _position;

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        // TODO: Consistency checks
        public void AddEdge(Edge edge)
        {
            m_Edges.Add(edge);
        }

        // TODO: Consistency checks
        public void RemoveEdge(Edge edge)
        {
            m_Edges.Remove(edge);
        }

        public virtual PortSettings GetInputPortSettings() => new InputPortSettings();
        public virtual PortSettings GetOutputPortSettings() => new OutputPortSettings();

        public abstract class PortSettings
        {
            public virtual Orientation GetOrientation() => Orientation.Horizontal;
            public abstract Direction GetDirection();
            public virtual Port.Capacity GetCapacity() => Port.Capacity.Single;
            public virtual Type GetPortType() => typeof(bool);
            public virtual string GetName() => string.Empty;
        }

        public class InputPortSettings : PortSettings
        {
            public override Direction GetDirection() => Direction.Input;

            public override string GetName() => "Input";
        }

        public class OutputPortSettings : PortSettings
        {
            public override Direction GetDirection() => Direction.Output;
            
            public override string GetName() => "Output";
        }
#endif
    }
}