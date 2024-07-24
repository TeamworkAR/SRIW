using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideBasicSplashScreenUIAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.BasicSplashScreenPanel.Hide();
        }
    }
}