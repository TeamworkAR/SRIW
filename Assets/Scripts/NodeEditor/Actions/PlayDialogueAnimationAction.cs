using System;
using Core;
using Data;
using Data.CharacterData;
using Data.ScriptableObjectVariables;
using DialogueSystem;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class PlayDialogueAnimationAction : Action
    {
        [SerializeField] private CharacterData m_Character = null;

        [SerializeField] private AnimatorOverrideController m_ControllerOverride = null;

        [SerializeField] private AnimatorOverrideController m_LipsControllerOverride = null;

        [SerializeField] private CharacterCamera.CameraPositions m_CameraPosition = CharacterCamera.CameraPositions.None;

        [SerializeField] private Consts.Moods.MoodData m_Mood = null;

        [SerializeField] private bool m_IsLastDialogueNode = false;

        [SerializeField] private bool m_AreLipsMoving = false;

        public override void Execute()
        {
            if (DialogueManager.Instance.Characters.ContainsKey(m_Character) == false)
            {
                Debug.LogError($"{m_Character.name} can't play a line as it's not instantiated");
                
                return;
            }

            DialogueManager.Instance.Characters[m_Character].PlayAnimation(m_ControllerOverride, m_LipsControllerOverride, m_CameraPosition, m_Mood, m_IsLastDialogueNode, m_AreLipsMoving);
        }
    }
}