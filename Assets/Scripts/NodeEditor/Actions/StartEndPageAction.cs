using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartEndPageAction : Action
    {
        public override void Execute() => MainGUI.Instance.MEndPageUI.Show();
    }
}