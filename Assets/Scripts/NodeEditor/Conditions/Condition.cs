using System;

namespace NodeEditor.Conditions
{
    // TODO: Consider a string representation to be included in graph view
    [Serializable]
    public abstract class Condition
    {
        public abstract bool Evaluate();

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}