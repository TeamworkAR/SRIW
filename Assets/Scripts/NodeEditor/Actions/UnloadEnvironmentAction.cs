using System;
using Managers;

namespace NodeEditor.Actions
{
    [Serializable]
    public class UnloadEnvironmentAction : Action
    { 
        public override void Execute()
        {
            EnvironmentManager.Instance.UnloadEnvironment();
        }
    }
}