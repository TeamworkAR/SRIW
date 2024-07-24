using System;
using System.Collections.Generic;
using CareBoo.Serially;
using UnityEngine;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class AndCondition : Condition
    {
        [SerializeReference, ShowSerializeReference] private List<Condition> m_Conditions = new List<Condition>(0);
        
        public override bool Evaluate()
        {
            return m_Conditions.TrueForAll(c => c.Evaluate());
        }

        public override string ToString()
        {
            string toString = String.Empty;

            for (int i = 0; i < m_Conditions.Count; i++)
            {
                if (i < m_Conditions.Count - 1)
                {
                    toString += $"{m_Conditions[i]} AND ";   
                }
                else
                {
                    toString += $"{m_Conditions[i]}";
                }
            }

            return toString;
        }
    }
}