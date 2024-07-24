using System;
using Data.ScenarioSettings;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public sealed class ShowSequencialInfoPostIt : Action
    {
        [SerializeField] private SequencialInfoCardDataWrapper infoCardDataWrapper = null;
        
        public override void Execute()
        {
            MainGUI.Instance.SequencialInfoPostIt.ShowStandalone(infoCardDataWrapper);
        }
    }
}