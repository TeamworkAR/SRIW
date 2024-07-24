using System;
using Core;
using UnityEngine;

namespace Data.ScenarioSettings
{
    [Serializable]
    public class EndPageExtension : ScenarioSettings.ScenarioExtension
    {
        [SerializeField] private CharacterData.CharacterData m_Character = null;

        [SerializeField] private Consts.Moods.Mood m_Mood = Consts.Moods.Mood.None;
        
        public CharacterData.CharacterData Character => m_Character;

        public Consts.Moods.Mood Mood => m_Mood;
    }
}