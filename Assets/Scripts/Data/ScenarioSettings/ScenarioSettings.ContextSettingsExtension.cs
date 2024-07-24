using Managers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Data.ScenarioSettings
{
    public partial class ScenarioSettings
    {
        [Serializable]
        public class ContextSettingsExtension : ScenarioExtension
        {
            [SerializeField] private List<CharacterData.CharacterData> m_LeftCharacters = null;
            
            [SerializeField] private List<CharacterData.CharacterData> m_RightCharacters = null;

            [SerializeField] private LocalizedString m_RecapText = null;
            
            public List<CharacterData.CharacterData> LeftCharacters => m_LeftCharacters;

            public List<CharacterData.CharacterData> RightCharacters => m_RightCharacters;

            public string RecapText => LocalizationManager.Instance.GetLocalizedValue(m_RecapText);
        }
    }
}