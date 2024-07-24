using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartConclusionsAction : Action
    {
        public override void Execute() => MainGUI.Instance.MConclusionsUI.Show();
    }
}