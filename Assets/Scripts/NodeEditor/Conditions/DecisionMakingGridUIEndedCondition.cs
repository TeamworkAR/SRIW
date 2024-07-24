using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class DecisionMakingGridUIEndedCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.DecisionMakingGridUI.IsDone();
    }
}