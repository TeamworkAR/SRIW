using Data.ScenarioSettings;
using System;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartBasicInfoPanelAction : Action
    {
        [SerializeField] private BasicInfoPanelDataWrapper infoPanelData = null;

        public override void Execute()
        {
            MainGUI.Instance.BasicInfoPanel.ShowText(infoPanelData);
        }
    }
}