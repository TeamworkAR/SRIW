using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class DecisionMaking4GridUIEndedCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.DecisionMaking4GridUI.IsDone();
    }
}