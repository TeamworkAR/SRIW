using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Localization;

namespace Data.ScenarioSettings
{
    public partial class ScenarioSettings
    {
        [Serializable]
        public class DecisionMakingExtension : ScenarioExtension
        {
            [SerializeField] private CharacterData.CharacterData m_Chatacter = null;
            
            [SerializeField] private List<Tile> m_Tiles = new List<Tile>(0);

            [SerializeField] private List<AttemptClipData> m_WrongAttemptClips = new List<AttemptClipData>();

            [SerializeField] private AttemptClipData m_RightAttemptClip = null;
            
            public List<Tile> Tiles => m_Tiles;

            public CharacterData.CharacterData Chatacter => m_Chatacter;

            public AttemptClipData RightAttemptClip => m_RightAttemptClip;

            public List<AttemptClipData> WrongAttemptClips => m_WrongAttemptClips;

            [Serializable]
            public class Entry
            {
                [SerializeField] private bool b_IsRightEntry = false;
                
                [SerializeField] private LocalizedString m_Text;

                [SerializeField] private AttemptClipData m_AttemptClipOverride = null;
                
                [Tooltip("Show a sprite in a tile if authored")]
                [SerializeField] private Sprite m_Sprite = null;

                public string GetText() => LocalizationManager.Instance.GetLocalizedValue(m_Text);

                public Sprite Sprite => m_Sprite;

                public bool IsRightEntry => b_IsRightEntry;

                public AttemptClipData AttemptClipOverride => m_AttemptClipOverride;
            }
            
            [Serializable]
            public class Tile
            {
                [SerializeField] private List<Entry> m_Entries = new List<Entry>(0);

                public List<Entry> Entries => m_Entries;
            }

            [Serializable]
            public class AttemptClipData
            {
                [SerializeField] private AudioClip m_AudioClip = null;
                [SerializeField] private LocalizedString m_SubTitles = null;

                public AudioClip Audio => m_AudioClip;

                public string SubTitles => LocalizationManager.Instance.GetLocalizedValue(m_SubTitles);
            }
        }
    }
}