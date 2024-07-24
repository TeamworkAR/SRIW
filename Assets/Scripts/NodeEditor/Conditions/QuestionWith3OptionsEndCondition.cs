using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace NodeEditor.Conditions
{
    [System.Serializable]
    public class QuestionWith3OptionsEndCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.QuestionWith3OptionPanel.IsDone();
    }
}
