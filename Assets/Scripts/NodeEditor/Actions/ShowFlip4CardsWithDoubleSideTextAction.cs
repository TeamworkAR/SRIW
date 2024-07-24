using Data.ScenarioSettings;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [System.Serializable]
    public class ShowFlip4CardsWithDoubleSideTextAction : Action
    {
        [SerializeField] private Flip4CardsWithDoubleSideTextDataWrapper infoPanelData = null;

        public override void Execute()
        {
            MainGUI.Instance.Flip4CardsWithDoubleSideTextPanel.ShowText(infoPanelData);
        }
    }
}