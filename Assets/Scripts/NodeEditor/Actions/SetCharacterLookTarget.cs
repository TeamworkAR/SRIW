using System;
using Data.CharacterData;
using Data.ScriptableObjectVariables;
using DialogueSystem;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class SetCharacterLookTarget : Action
    {
        [SerializeField] private CharacterData m_Character = null;
        
        [SerializeField] private TransformVariable m_LookTarget = null;
        
        public override void Execute()
        {
            if (DialogueManager.Instance.Characters.ContainsKey(m_Character) == false)
            {
                Debug.LogError($"{m_Character.name} can't play a line as it's not instantiated");
                
                return;
            }

            DialogueManager.Instance.Characters[m_Character].SetLookTarget(m_LookTarget);
        }
    }
}