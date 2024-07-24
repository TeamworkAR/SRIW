using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideSingleFlipPanelAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.SingleFlipPanel.Interrupt();
        }
    }
}