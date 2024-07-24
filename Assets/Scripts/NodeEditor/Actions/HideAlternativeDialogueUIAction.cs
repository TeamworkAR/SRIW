using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideAlternativeDialogueUIAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.MAlternativeDialogueUI.Hide();
        }
    }
}