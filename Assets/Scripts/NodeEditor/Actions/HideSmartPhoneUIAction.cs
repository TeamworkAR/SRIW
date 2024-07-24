using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideSmartPhoneUIAction : Action
    { 
        public override void Execute()
        {
            MainGUI.Instance.MSmartphoneUI.Hide();
        }
    }
}