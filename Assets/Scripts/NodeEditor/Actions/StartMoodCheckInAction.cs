using System;
using System.Collections.Generic;
using Data.CharacterData;
using UI;
using UI.MoodCheckIn;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartMoodCheckInAction : Action
    {
        [SerializeField] private MoodCheckInEntry m_Entry = null;
        
        public override void Execute()
        {
            MainGUI.Instance.MMoodCheckInUI.Show(m_Entry);
        }
    }
        
    [Serializable]
    public class MoodCheckInEntry
    {
        [SerializeField] private CharacterData m_CharacterData = null;

        [SerializeField] private List<MoodCheckInUI.Mood> m_Moods = new List<MoodCheckInUI.Mood>(0);

        public CharacterData Character => m_CharacterData;

        public List<MoodCheckInUI.Mood> Moods => m_Moods;    
    }
}