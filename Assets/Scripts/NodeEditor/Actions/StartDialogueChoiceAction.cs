using System;
using UI;
using Data.CharacterData;
using UI.DialogueChoice;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartDialogueChoiceAction : Action
    {
        [SerializeField] private DialogueChoiceCollection m_Choices = new DialogueChoiceCollection();

        [SerializeField] private CharacterData m_Character = null;

        public override void Execute() => MainGUI.Instance.MDialogueChoiceUI.Show(m_Choices, m_Character);
    }
}