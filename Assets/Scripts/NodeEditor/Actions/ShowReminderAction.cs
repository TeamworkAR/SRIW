using Data.ScenarioSettings;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [System.Serializable]
    public class ShowReminderAction : Action
    {
        [SerializeField] private ReminderDataWrapper infoPanelData = null;

        public override void Execute()
        {
            MainGUI.Instance.ReminderPanel.ShowText(infoPanelData);
        }
    }
}