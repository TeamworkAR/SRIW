using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public sealed class HideEndPageAction : Action
    {
        public override void Execute() => MainGUI.Instance.MEndPageUI.Hide();
    }
}