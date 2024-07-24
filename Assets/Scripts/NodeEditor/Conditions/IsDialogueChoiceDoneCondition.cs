using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class IsDialogueChoiceDoneCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.MDialogueChoiceUI.IsDone();
    }
}