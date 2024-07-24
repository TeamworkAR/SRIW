using Data.ScenarioSettings;
using System;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartBasicCarouselPanelAction : Action
    {
        [SerializeField] private CarouselPanelDataWrapper dataWrapper;

        public override void Execute()
        {
            MainGUI.Instance.BasicCarouselPanel.Show(dataWrapper);
        }
    }
}