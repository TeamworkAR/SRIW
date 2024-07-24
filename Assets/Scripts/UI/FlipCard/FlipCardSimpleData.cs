using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using static Data.ScenarioSettings.ScenarioSettings.DecisionMakingExtension;

[System.Serializable]
public class FlipCardSimpleData
{

    [SerializeField] private LocalizedString frontTextEntry;

    [SerializeField] private LocalizedString flippedTextEntry;

    [SerializeField] private AttemptClipData m_AttemptClipOverride = null;

    public LocalizedString FrontText => frontTextEntry;
    public LocalizedString FlippedText => flippedTextEntry;

    public AttemptClipData AttemptClipOverride => m_AttemptClipOverride;
}
