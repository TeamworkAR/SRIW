using Data.ScenarioSettings;
using System;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartBasicCharacterOnLeftPanelAction : Action
    {
        [SerializeField] private BasicCharacterOnLeftDataWrapper infoPanelData = null;

        public override void Execute()
        {
            MainGUI.Instance.BasicCharacterOnLeftPanel.ShowText(infoPanelData);
        }
    }
}