using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public sealed class HideSequencialInfoPostIt : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.SequencialInfoPostIt.Hide();
        }
    }
}