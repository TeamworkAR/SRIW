using Data.CharacterData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using static Data.ScenarioSettings.ScenarioSettings.DecisionMakingExtension;

[System.Serializable]
public class SingleFlipPanelData
{
    [SerializeField] private CharacterData m_Chatacter = null;

    [SerializeField] private FlipCardSimpleData m_Tile = null;

    [SerializeField] private LocalizedString persistentTitleCard = null;

    [SerializeField] private LocalizedString subTitleCard = null;

    [SerializeField] private LocalizedString infoCard = null;

    public FlipCardSimpleData Tile => m_Tile;

    public CharacterData Chatacter => m_Chatacter;

    public LocalizedString TitleString => persistentTitleCard;

    public LocalizedString SubTitleString => subTitleCard;

    public LocalizedString InfoString => infoCard;


}
