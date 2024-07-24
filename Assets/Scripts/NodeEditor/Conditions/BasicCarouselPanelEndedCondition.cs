using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class BasicCarouselPanelEndedCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.BasicCarouselPanel.IsDone();
    }
}