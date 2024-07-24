using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public sealed class DeactivateClueBookAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.MClueBookUI.DeactivateClueBook();
        }
    }
}