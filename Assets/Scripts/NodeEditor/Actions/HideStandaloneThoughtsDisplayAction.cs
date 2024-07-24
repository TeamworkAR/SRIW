using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public sealed class HideStandaloneThoughtsDisplayAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.StandaloneThoughtsDisplay.Hide();
        }
    }
}