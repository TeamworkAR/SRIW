using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideSequencialInfoHorizontalPanelUIAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.SequencialInfoHorizontalPanel.Hide();
        }
    }
}