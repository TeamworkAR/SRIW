using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideDecisionMakingAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.MDecisionMakingUI.Interrupt();
        }
    }
}