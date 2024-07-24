using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Node = NodeEditor.Nodes.Node;

namespace NodeEditor.Editor
{
    public sealed class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public static Action<NodeView> onNodeViewSelected;
        
        private Node m_Data;

        private Port m_Input = null;
        private Port m_Output = null;
        
        public Node Data => m_Data;

        public NodeView(Node data)
        {
            m_Data = data;
            
            this.title = data.GetType().Name;

            this.viewDataKey = m_Data.Guid;
            
            style.left = m_Data.Position.x;
            style.top = m_Data.Position.y;
            
            CreateInputPorts();
            CreateOutputPorts();
        }

        // TODO: Check if I can restore edges inside the constructor
        public EdgeView ConnectChild(NodeView child) => m_Output.ConnectTo<EdgeView>(child.m_Input);

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            
            m_Data.SetPosition(new Vector2(newPos.xMin,newPos.yMin));
        }

        public override void OnSelected()
        {
            base.OnSelected();
            
            onNodeViewSelected?.Invoke(this);
        }

        // TODO: Implement something related to PortTypes
        // TODO: Implement a button to add ports
        private void CreateInputPorts()
        {
            var settings = m_Data.GetInputPortSettings();
            m_Input = Port.Create<EdgeView>(settings.GetOrientation(), settings.GetDirection(), settings.GetCapacity(),
                settings.GetPortType());
            m_Input.portName = settings.GetName();
            
            inputContainer.Add(m_Input);
        }

        private void CreateOutputPorts()
        {
            var settings = m_Data.GetOutputPortSettings();
            m_Output = Port.Create<EdgeView>(settings.GetOrientation(), settings.GetDirection(), settings.GetCapacity(),
                settings.GetPortType());
            m_Output.portName = settings.GetName();
            
            outputContainer.Add(m_Output);   
        }
    }
}