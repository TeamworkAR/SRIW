using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideDecisionMakingGridUIAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.DecisionMakingGridUI.Interrupt();
        }
    }
}