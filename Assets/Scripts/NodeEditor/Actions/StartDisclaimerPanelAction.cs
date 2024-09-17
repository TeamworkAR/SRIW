using Data.ScenarioSettings;
using System;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartDisclaimerPanelAction : Action
    {
        [SerializeField] private DisclaimerPanelDataWrapper infoPanelData = null;

        public override void Execute()
        {
            MainGUI.Instance.DisclaimerPanel.ShowText(infoPanelData);
        }
    }
}