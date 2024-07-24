using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartDecisionMakingAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.MDecisionMakingUI.Show();
        }
    }
}