using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideBasicCharacterOnLeftPanelAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.BasicCharacterOnLeftPanel.Interrupt();
        }
    }
}