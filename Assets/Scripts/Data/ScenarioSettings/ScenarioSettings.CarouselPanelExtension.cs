using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using static Data.ScenarioSettings.ScenarioSettings.BasicCarouselPanelExtension;

namespace Data.ScenarioSettings
{
    public partial class ScenarioSettings
    {
        [Serializable]
        public class CarouselPanelExtension : ScenarioSettings
        {

        }
    }

    [Serializable]
    public class CarouselPanelData
    {
        [SerializeField] private List<BasicCarouselEntry> entries = new List<BasicCarouselEntry>(0);
        public List<BasicCarouselEntry> Entries => entries;
    }
}