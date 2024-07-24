using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideMoodCheckInAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.MMoodCheckInUI.Interrupt();
        }
    }
}