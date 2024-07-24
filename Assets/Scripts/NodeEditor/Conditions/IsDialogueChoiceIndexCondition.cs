using System;
using UI;
using UnityEngine;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class IsDialogueChoiceIndexCondition : Condition
    {
        [SerializeField] private int m_IndexToCheck = 0;

        public override bool Evaluate() => MainGUI.Instance.MDialogueChoiceUI.IsChoiceIndex(m_IndexToCheck);
    }
}