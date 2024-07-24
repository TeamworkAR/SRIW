using System;
using NodeEditor.Conditions;

namespace DialogueSystem
{
    [Serializable]
    public class IsDialogueDoneCondition : Condition
    {
        public override bool Evaluate() => DialogueManager.Instance.IsDone;
    }
}