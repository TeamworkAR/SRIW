using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class DeactivateYellowBackground : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.MBackgroundUI.DeactivateYellowBackground();
        }
    }
}