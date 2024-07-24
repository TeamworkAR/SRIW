using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class MoodCheckInEndedCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.MMoodCheckInUI.IsDone();
    }
}