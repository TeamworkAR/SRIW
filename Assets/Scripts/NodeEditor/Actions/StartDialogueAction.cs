using System;
using Data;
using Data.DialogueData;
using DialogueSystem;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartDialogueAction : Action
    {
        [SerializeField] private DialogueData m_Dialogue = null;
        
        public override void Execute()
        {
            DialogueManager.Instance.StartDialogue(m_Dialogue);
        }
    }
}