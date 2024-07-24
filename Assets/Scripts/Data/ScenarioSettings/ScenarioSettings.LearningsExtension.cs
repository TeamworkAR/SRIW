using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Data.ScenarioSettings
{
    public partial class ScenarioSettings
    {
        [Serializable]
        public class LearningsExtension : ScenarioExtension
        {
            [SerializeField] private CharacterData.CharacterData m_LearningsCharacter = null;

            [SerializeField] private LocalizedString m_LearningsIntro = null;

            [SerializeField] private List<LearningsEntry> m_Entries = new List<LearningsEntry>(0);
            
            public CharacterData.CharacterData LearningsCharacter => m_LearningsCharacter;
            
            public LocalizedString LearningsIntro => m_LearningsIntro;

            public List<LearningsEntry> Entries => m_Entries;

            [Serializable]
            public class LearningsEntry
            {
                [SerializeField] private CharacterData.CharacterData m_Character = null;
                
                [SerializeField] private LocalizedString m_Title = null;
                
                [SerializeField] private LocalizedString m_Content = null;

                public LocalizedString Title => m_Title;

                public LocalizedString Content => m_Content;

                public CharacterData.CharacterData Character => m_Character;
            }
        }
    }
}