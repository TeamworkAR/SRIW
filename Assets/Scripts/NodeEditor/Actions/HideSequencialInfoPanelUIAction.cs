using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public sealed class HideSequencialInfoPanelUIAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.SequencialInfoPanel.Hide();
        }
    }
}