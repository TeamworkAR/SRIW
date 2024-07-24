using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public sealed class SequencialInfoPanelEndedCondition : Condition
    {
        public override bool Evaluate()
        {
            return MainGUI.Instance.SequencialInfoPanel.SequencialInfoEnded;
        }
    }
}