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
        public class SequencialInfoHorizontalPanelExtension : ScenarioSettings
        {

        }
    }

    [Serializable]
    public class SequencialInfoHorizontalCardData
    {
        [SerializeField] private LocalizedString persistentTitleCard = null;

        [SerializeField] private LocalizedString conclusionTitleString = null;

        [SerializeField] private LocalizedString conclusionString = null;

        [SerializeField] private CharacterData.CharacterData owner = null;

        [SerializeField] private List<InfoCard> cards = new List<InfoCard>(0);

        public LocalizedString TitleString => persistentTitleCard;

        public LocalizedString ConclusionString => conclusionString;

        public LocalizedString ConclusionTitleString => conclusionTitleString;
        public List<InfoCard> Cards => cards;
    }
}