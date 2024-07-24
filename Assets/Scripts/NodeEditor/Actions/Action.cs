using System;

namespace NodeEditor.Actions
{
    [Serializable]
    public abstract class Action
    {
        public abstract void Execute();
    }
}