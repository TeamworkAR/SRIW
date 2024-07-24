using System;
using Data;
using Data.CharacterData;
using NodeEditor.Conditions;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class IsCharacterTalkingCondition : Condition
    {
        [SerializeField] private CharacterData m_Character = null;
        
        public override bool Evaluate() => DialogueManager.Instance.Characters[m_Character].IsAnimating;
    }
}