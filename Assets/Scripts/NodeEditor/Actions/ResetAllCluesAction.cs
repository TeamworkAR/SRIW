using System;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class ResetAllCluesAction : Action
    {
        public override void Execute()
        {
            // GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.ClueCollectionExtension>().ResetAllClues();

            Debug.Log($"{nameof(ResetAllCluesAction)} called. It does nothing as we are not re-locking clues anymore");
        }
    }
}