using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideBasicInfoPanelAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.BasicInfoPanel.Interrupt();
        }
    }
}