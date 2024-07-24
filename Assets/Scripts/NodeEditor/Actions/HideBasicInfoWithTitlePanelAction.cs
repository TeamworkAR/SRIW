using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideBasicInfoWithTitlePanelAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.BasicInfoWithTitlePanel.Interrupt();
        }
    }
}