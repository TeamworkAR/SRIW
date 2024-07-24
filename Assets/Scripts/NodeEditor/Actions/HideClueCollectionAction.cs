using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideClueCollectionAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.MClueCollectionUI.Interrupt();
        }
    }
}