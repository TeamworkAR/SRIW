using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class ActivateYellowBackground : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.MBackgroundUI.ActivateYellowBackground();
        }
    }
}