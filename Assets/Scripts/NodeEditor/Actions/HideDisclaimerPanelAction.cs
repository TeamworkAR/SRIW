using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideDisclaimerPanelAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.DisclaimerPanel.Interrupt();
        }
    }
}