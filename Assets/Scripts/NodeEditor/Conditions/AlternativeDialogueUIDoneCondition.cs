using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class AlternativeDialogueUIDoneCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.MAlternativeDialogueUI.IsDone();
    }
}