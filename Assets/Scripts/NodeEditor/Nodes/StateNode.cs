#if UNITY_EDITOR
    using UnityEditor.Experimental.GraphView;
#endif

namespace NodeEditor.Nodes
{
    public class StateNode : Node
    {
#if UNITY_EDITOR
        public override PortSettings GetOutputPortSettings() => new StateNodeOutputPortSettings();

        public override PortSettings GetInputPortSettings() => new StateNodeInputPortSettings();

        public class StateNodeOutputPortSettings : OutputPortSettings
        {
            public override Port.Capacity GetCapacity() => Port.Capacity.Multi;
        }

        public class StateNodeInputPortSettings : InputPortSettings
        {
            public override Port.Capacity GetCapacity() => Port.Capacity.Multi;
        }
#endif
    }
}