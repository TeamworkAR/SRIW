using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public sealed class HideSequencialConclusionInfoPanelUIAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.SequencialConclusionInfoPanel.Hide();
        }
    }
}