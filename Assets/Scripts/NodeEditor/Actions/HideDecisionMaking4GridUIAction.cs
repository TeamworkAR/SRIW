using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideDecisionMaking4GridUIAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.DecisionMaking4GridUI.Interrupt();
        }
    }
}