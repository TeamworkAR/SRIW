using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class DisclaimerPanelEndedCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.DisclaimerPanel.IsDone();
    }
}