using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class SingleFlipPanelEndedCondition : Condition
    {
        public override bool Evaluate()
        {
            return MainGUI.Instance.SingleFlipPanel.IsDone();
        }
    }
}