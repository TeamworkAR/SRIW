using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public sealed class SequencialConclusionInfoPanelEndedCondition : Condition
    {
        public override bool Evaluate()
        {
            return MainGUI.Instance.SequencialConclusionInfoPanel.SequencialInfoEnded;
        }
    }
}