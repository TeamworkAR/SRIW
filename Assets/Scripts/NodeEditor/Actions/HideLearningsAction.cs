using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideLearningsAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.MLearningsUI.Interrupt();
        }
    }
}