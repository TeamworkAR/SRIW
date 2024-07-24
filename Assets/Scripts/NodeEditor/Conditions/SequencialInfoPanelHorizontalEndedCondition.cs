using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public sealed class SequencialInfoPanelHorizontalEndedCondition : Condition
    {
        public override bool Evaluate()
        {
            return MainGUI.Instance.SequencialInfoHorizontalPanel.SequencialInfoEnded;
        }
    }
}