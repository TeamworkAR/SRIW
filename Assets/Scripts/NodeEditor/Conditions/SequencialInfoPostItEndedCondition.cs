using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public sealed class SequencialInfoPostItEndedCondition : Condition
    {
        public override bool Evaluate()
        {
            return MainGUI.Instance.SequencialInfoPostIt.SequencialInfoEnded;
        }
    }
}