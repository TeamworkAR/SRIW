using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class SmartPhoneUIDoneCondition : Condition
    {
        public override bool Evaluate()
        {
            return MainGUI.Instance.MSmartphoneUI.IsOnScreen() == false;
        }
    }
}