using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class ContextSettingsEndedCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.MContextSettingsUI.IsDone();
    }
}