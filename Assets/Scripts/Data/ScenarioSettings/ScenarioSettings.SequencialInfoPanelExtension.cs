using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Data.ScenarioSettings
{
    public partial class ScenarioSettings
    {
        [Serializable]
        public class SequencialInfoPanelExtension : ScenarioSettings
        {

        }
    }

    [Serializable]
    public class SequencialInfoCardData
    {
        [SerializeField] private LocalizedString persistentTitleCard = null;

        [SerializeField] private CharacterData.CharacterData owner = null;

        [SerializeField] private List<InfoCard> cards = new List<InfoCard>(0);

        public LocalizedString TitleString => persistentTitleCard;

        public CharacterData.CharacterData Owner => owner;

        public List<InfoCard> Cards => cards;
    }
}