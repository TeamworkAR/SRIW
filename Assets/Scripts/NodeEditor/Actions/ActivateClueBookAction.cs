using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public sealed class ActivateClueBookAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.MClueBookUI.ActivateClueBook();
        }
    }
}