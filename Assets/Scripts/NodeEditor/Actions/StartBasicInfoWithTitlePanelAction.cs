using Data.ScenarioSettings;
using System;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartBasicInfoWithTitlePanelAction : Action
    {
        [SerializeField] private BasicInfoWithTitlePanelDataWrapper infoPanelData = null;

        public override void Execute()
        {
            MainGUI.Instance.BasicInfoWithTitlePanel.ShowText(infoPanelData);
        }
    }
}