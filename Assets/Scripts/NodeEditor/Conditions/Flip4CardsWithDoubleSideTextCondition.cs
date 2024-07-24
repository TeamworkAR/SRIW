using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace NodeEditor.Conditions
{
    [System.Serializable]
    public class Flip4CardsWithDoubleSideTextCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.Flip4CardsWithDoubleSideTextPanel.IsDone();
    }
}