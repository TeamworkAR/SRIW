using System;
using UI.DecisionMaking;
using UnityEngine;
using UnityEngine.Localization;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class IsAnyDecisionMakingTileResult : Condition
    {
        [SerializeField] private LocalizedString m_Test = null;

        public override bool Evaluate() => DecisionMakingUI.Result != null &&
                                           DecisionMakingUI.Result.ResultTexts.Exists(r =>
                                               string.CompareOrdinal(r, m_Test.GetLocalizedString()) == 0);
    }
}