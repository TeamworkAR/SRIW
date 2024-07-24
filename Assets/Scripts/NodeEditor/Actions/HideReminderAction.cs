using Data.ScenarioSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideReminderAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.ReminderPanel.Interrupt();
        }
    }
}