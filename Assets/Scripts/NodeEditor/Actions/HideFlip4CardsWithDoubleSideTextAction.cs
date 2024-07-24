using Data.ScenarioSettings;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideFlip4CardsWithDoubleSideTextAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.Flip4CardsWithDoubleSideTextPanel.Interrupt();
        }
    }
}