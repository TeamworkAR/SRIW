using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using static Data.ScenarioSettings.ScenarioSettings.DecisionMakingExtension;

[System.Serializable]
public class FlipCardDecisionData
{
    [SerializeField] private bool b_IsRightEntry = false;

    [SerializeField] private LocalizedString frontTextEntry;

    [SerializeField] private LocalizedString flippedTextEntry;

    [SerializeField] private LocalizedString incorrectAnswerResponse;

    [SerializeField] private AttemptClipData m_AttemptClipOverride = null;

    public LocalizedString FrontText => frontTextEntry;
    public LocalizedString FlippedText => flippedTextEntry;
    public LocalizedString IncorrectAnswerResponse => incorrectAnswerResponse;

    public bool IsRightEntry => b_IsRightEntry;

    public AttemptClipData AttemptClipOverride => m_AttemptClipOverride;
}
