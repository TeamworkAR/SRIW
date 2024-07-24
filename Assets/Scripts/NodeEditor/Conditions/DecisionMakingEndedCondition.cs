using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class DecisionMakingEndedCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.MDecisionMakingUI.IsDone();
    }
}