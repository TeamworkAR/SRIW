using System;
using Data.ScenarioSettings;
using Managers;

namespace NodeEditor.Actions
{
    [Serializable]
    public class UnlockAllCluesAction : Action
    {
        public override void Execute()
        {
            GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.ClueCollectionExtension>().UnlockAll();
        }
    }
}