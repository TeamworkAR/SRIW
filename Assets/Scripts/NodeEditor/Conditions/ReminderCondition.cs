using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace NodeEditor.Conditions
{
    [System.Serializable]
    public class ReminderCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.ReminderPanel.IsDone();
    }
}