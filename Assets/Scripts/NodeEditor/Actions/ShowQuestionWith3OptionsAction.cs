using Data.ScenarioSettings;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [System.Serializable]
    public class ShowQuestionWith3OptionsAction : Action
    {
        [SerializeField] private QuestionWith3OptionsDataWrapper infoPanelData = null;

        public override void Execute()
        {
            MainGUI.Instance.QuestionWith3OptionPanel.ShowText(infoPanelData);
        }
    }
}