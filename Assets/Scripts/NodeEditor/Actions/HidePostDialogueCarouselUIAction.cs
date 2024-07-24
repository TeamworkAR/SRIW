using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HidePostDialogueCarouselUIAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.MPostDialogueCarouselUI.Hide();
        }
    }
}