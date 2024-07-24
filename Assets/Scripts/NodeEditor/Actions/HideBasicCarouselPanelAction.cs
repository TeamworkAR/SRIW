using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideBasicCarouselPanelAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.BasicCarouselPanel.Interrupt();
        }
    }
}