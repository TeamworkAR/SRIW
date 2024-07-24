using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideContextSettingsAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.MContextSettingsUI.Interrupt();
        }
    }
}