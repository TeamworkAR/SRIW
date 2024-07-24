using System;
using Data.ScenarioSettings;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public sealed class ShowSingleFlipPanelUIAction : Action
    {
        [SerializeField] private SingleFlipCardDataWrapper infoCardDataWrapper = null;
        
        public override void Execute()
        {
            MainGUI.Instance.SingleFlipPanel.ShowWithWrapperData(infoCardDataWrapper);
        }
    }
}