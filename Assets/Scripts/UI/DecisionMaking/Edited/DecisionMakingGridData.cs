using Data.CharacterData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using static Data.ScenarioSettings.ScenarioSettings.DecisionMakingExtension;

[System.Serializable]
public class DecisionMakingGridData 
{
    [SerializeField] private CharacterData m_Chatacter = null;

    [SerializeField] private LocalizedString circleTopString = null;

    [SerializeField] private LocalizedString circleBottomString = null;

    [SerializeField] private LocalizedString lowerString = null;

    [SerializeField] private List<FlipCardDecisionData> m_Tiles = new List<FlipCardDecisionData>(0);

    [SerializeField] private List<AttemptClipData> m_WrongAttemptClips = new List<AttemptClipData>();

    [SerializeField] private AttemptClipData m_RightAttemptClip = null;

    public LocalizedString CircleTopString => circleTopString;

    public LocalizedString CircleBottomString => circleBottomString;

    public LocalizedString LowerString => lowerString;

    public List<FlipCardDecisionData> Tiles => m_Tiles;

    public CharacterData Chatacter => m_Chatacter;

    public AttemptClipData RightAttemptClip => m_RightAttemptClip;

    public List<AttemptClipData> WrongAttemptClips => m_WrongAttemptClips;
}
