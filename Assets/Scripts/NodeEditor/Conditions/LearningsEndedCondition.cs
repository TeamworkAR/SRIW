using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class LearningsEndedCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.MLearningsUI.IsDone();
    }
}