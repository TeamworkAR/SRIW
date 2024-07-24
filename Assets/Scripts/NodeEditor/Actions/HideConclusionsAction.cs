using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideConclusionsAction : Action
    {
        public override void Execute() => MainGUI.Instance.MConclusionsUI.Hide();
    }
}