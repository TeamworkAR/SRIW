using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace NodeEditor.Conditions
{
    [System.Serializable]
    public class QuestionWith4OptionsEndCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.QuestionWith4OptionPanel.IsDone();
    }
}
