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
        public class BasicInfoWithTitlePanelExtension : ScenarioSettings
        {

        }
    }

    [Serializable]
    public class BasicInfoWithTitlePanelData
    {
        [SerializeField] private LocalizedString titleString = null;
        [SerializeField] private LocalizedString infoString = null;

        [SerializeField] private bool showTextBackground = true;

        public LocalizedString TitleString => titleString;
        public LocalizedString InfoString => infoString;


        public bool ShowTextBackground => showTextBackground;

    }
}