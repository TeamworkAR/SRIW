using System;
using Core;
using Data.CharacterData;
using DialogueSystem;
using UnityEngine;
using UnityEngine.Localization;

namespace NodeEditor.Actions
{
    [Serializable]
    public class PlayDialogueLineAction : Action
    {
        [SerializeField] private CharacterData m_Character = null;

        [SerializeField] private AnimatorOverrideController m_ControllerOverride = null;

        [SerializeField] private AudioClip m_Clip = null;
        
        [SerializeField] private CharacterCamera.CameraPositions m_CameraPosition = CharacterCamera.CameraPositions.None;
        
        [SerializeField] private Consts.Moods.MoodData m_Mood = null;

        [SerializeField] private LocalizedString m_SubtitleText = null;

        [SerializeField] private bool m_IsLastDialogueNode = false;
        
        public override void Execute()
        {
            if (DialogueManager.Instance.Characters.ContainsKey(m_Character) == false)
            {
                Debug.LogError($"{m_Character.name} can't play a line as it's not instantiated");
                
                return;
            }

            DialogueManager.Instance.Characters[m_Character].PlayLine(m_ControllerOverride, m_Clip, m_CameraPosition, m_Mood, m_SubtitleText, m_IsLastDialogueNode);
        }
    }
}