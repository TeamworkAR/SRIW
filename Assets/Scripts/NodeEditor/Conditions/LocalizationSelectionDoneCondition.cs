using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class LocalizationSelectionDoneCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.MLocalizationUI.IsDone();
    }
}