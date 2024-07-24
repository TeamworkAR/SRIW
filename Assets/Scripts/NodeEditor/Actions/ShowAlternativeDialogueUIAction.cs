using System;
using Data.CharacterData;
using UI;
using UnityEngine;
using UnityEngine.Localization;

namespace NodeEditor.Actions
{
    [Serializable]
    public class ShowAlternativeDialogueUIAction : Action
    {
        [SerializeField] private CharacterData m_Character = null;

        [SerializeField] private LocalizedString m_Title = null;

        [SerializeField] private LocalizedString m_Text = null;

        public override void Execute()
        {
            MainGUI.Instance.MAlternativeDialogueUI.Show(m_Character, m_Title, m_Text);
        }
    }
}