using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class ClueCollectionEndedCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.MClueCollectionUI.IsDone();
    }
}