using System;
using CareBoo.Serially;
using UnityEngine;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class NotCondition : Condition
    {
        [SerializeReference, ShowSerializeReference] private Condition m_Condition = null;
        
        public override bool Evaluate()
        {
            return !m_Condition.Evaluate();
        }
        
        public override string ToString()
        {
            return $"NOT {m_Condition}";
        }
    }
}