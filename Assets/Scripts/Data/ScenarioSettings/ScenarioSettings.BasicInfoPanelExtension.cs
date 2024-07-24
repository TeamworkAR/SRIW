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
        public class BasicInfoPanelExtension : ScenarioSettings
        {

        }
    }

    [Serializable]
    public class BasicInfoPanelData
    {
        [SerializeField] private CharacterData.CharacterData owner = null;

        [SerializeField] private LocalizedString infoString = null;

        [SerializeField] private bool showTextBackground = true;

        public LocalizedString InfoString => infoString;

        public CharacterData.CharacterData Owner => owner;

        public bool ShowTextBackground => showTextBackground;

    }
}