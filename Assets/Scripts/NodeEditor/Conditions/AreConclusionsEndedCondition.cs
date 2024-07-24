using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class AreConclusionsEndedCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.MConclusionsUI.IsDone();
    }
}