using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class BasicInfoWithTitlePanelEndedCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.BasicInfoWithTitlePanel.IsDone();
    }
}