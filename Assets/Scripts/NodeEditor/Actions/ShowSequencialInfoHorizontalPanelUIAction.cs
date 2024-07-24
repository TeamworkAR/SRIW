using System;
using Data.ScenarioSettings;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public sealed class ShowSequencialInfoHorizontalPanelUIAction : Action
    {
        [SerializeField] private SequencialInfoHorizontalCardDataWrapper infoCardDataWrapper = null;
        
        public override void Execute()
        {
            MainGUI.Instance.SequencialInfoHorizontalPanel.ShowStandalone(infoCardDataWrapper);
        }
    }
}