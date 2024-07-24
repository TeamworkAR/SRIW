using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartClueCollectionAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.MClueCollectionUI.Show();
        }
    }
}