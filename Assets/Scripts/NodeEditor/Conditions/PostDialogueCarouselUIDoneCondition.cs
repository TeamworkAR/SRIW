using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class PostDialogueCarouselUIDoneCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.MPostDialogueCarouselUI.IsDone();
    }
}