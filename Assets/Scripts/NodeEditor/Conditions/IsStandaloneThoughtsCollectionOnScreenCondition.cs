using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public sealed class IsStandaloneThoughtsCollectionOnScreenCondition : Condition
    {
        public override bool Evaluate()
        {
            return MainGUI.Instance.StandaloneThoughtsDisplay.IsOnScreen();
        }
    }
}