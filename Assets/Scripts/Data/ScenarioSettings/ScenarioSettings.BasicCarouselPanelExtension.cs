using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Data.ScenarioSettings
{
    public partial class ScenarioSettings
    {
        [Serializable]
        public class BasicCarouselPanelExtension : ScenarioExtension
        {
            [SerializeField] private List<BasicCarouselEntry> entries = new List<BasicCarouselEntry>(0);
            public List<BasicCarouselEntry> Entries => entries;

            [Serializable]
            public class BasicCarouselEntry
            {
                [SerializeField] private CharacterData.CharacterData character = null;

                [SerializeField] private LocalizedString title = null;

                [SerializeField] private LocalizedString content = null;

                public LocalizedString Title => title;

                public LocalizedString Content => content;

                public CharacterData.CharacterData Character => character;
            }
        }
    }
}