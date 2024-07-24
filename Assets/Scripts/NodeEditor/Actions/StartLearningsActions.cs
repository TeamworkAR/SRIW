using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartLearningsActions : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.MLearningsUI.Show();
        }
    }
}