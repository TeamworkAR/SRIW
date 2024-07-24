using UnityEngine;

namespace UI.DecisionMaking
{
    public class DecisionMakingPanelContainer : PanelContainer<BaseUI>
    {
        public override bool IsDone()
        {
            return base.IsDone() && IsOnScreen() == false;
        }
    }
}