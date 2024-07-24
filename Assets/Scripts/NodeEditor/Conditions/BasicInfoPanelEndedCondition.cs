using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class BasicInfoPanelEndedCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.BasicInfoPanel.IsDone();
    }
}