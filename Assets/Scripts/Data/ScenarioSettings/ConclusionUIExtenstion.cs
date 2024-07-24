using System;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.Localization;

namespace Data.ScenarioSettings
{
    [Serializable]
    public class ConclusionUIExtenstion : ScenarioSettings.ScenarioExtension
    {
        [SerializeField] private LocalizedString m_ConclusionTitle = null;
        
        [SerializeField] private LocalizedString[] m_TileTitles = null;
        
        [SerializeField] private LocalizedString[] m_TileTexts = null;
        
        public string ConclusionTitle => LocalizationManager.Instance.GetLocalizedValue(m_ConclusionTitle);

        public string[] TileTitles => m_TileTitles.Select(s => LocalizationManager.Instance.GetLocalizedValue(s)).ToArray();
        
        public string[] TileTexts => m_TileTexts.Select(s => LocalizationManager.Instance.GetLocalizedValue(s)).ToArray();
    }
}