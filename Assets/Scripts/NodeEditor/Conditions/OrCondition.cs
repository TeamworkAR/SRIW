using System;
using System.Collections.Generic;
using CareBoo.Serially;
using UnityEngine;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class OrCondition : Condition
    {
        [SerializeReference, ShowSerializeReference] private List<Condition> m_Conditions = new List<Condition>(0);
        
        public override bool Evaluate()
        {
            return m_Conditions.Find(c => c.Evaluate()) != null;
        }
        
        public override string ToString()
        {
            string toString = String.Empty;

            for (int i = 0; i < m_Conditions.Count; i++)
            {
                if (i < m_Conditions.Count - 1)
                {
                    toString += $"{m_Conditions[i]} OR ";   
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