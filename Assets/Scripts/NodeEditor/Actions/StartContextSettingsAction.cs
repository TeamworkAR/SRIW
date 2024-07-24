using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartContextSettingsAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.MContextSettingsUI.Show();
        }
    }
}