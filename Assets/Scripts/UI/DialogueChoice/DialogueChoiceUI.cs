using Data.CharacterData;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace UI.DialogueChoice
{
    public class DialogueChoiceUI : BaseUICanvasGroup
    {
        [SerializeField] private TextMeshProUGUI m_Title = null;

        [SerializeField] private LocalizedString m_LocalizedString = null;

        [SerializeField] private RectTransform m_ChoicesContainer = null;

        [SerializeField] private DialogueChoiceDisplay m_DialogueChoiceDisplayTemplate = null;
        
        private DialogueChoiceCollection m_Choices = null;

        private DialogueChoice m_Selected = null;

        public override bool CanPause => false;

        public override bool IsDone() => base.IsDone() && m_Selected != null;

        private void OnEnable() => DialogueChoiceDisplay.OnChoiceSelected += OnChoiceSelected;

        private void OnDisable() => DialogueChoiceDisplay.OnChoiceSelected -= OnChoiceSelected;

        private List<DialogueChoiceDisplay> m_Views = new List<DialogueChoiceDisplay>(0);

        private CharacterData m_CharacterData = null;
        
        public void Show(DialogueChoiceCollection choices, CharacterData character)
        {
            m_Choices = choices;

            m_CharacterData = character;
            
            base.Show();
            this.GetComponent<AccessibleUIGroupRoot>().RefreshNextUpdate();
        }

        protected override void OnShowStart()
        {
            base.OnShowStart();
            
            m_Selected = null;

            m_Title.text = string.Format( m_Choices.GetTitle(), m_CharacterData.GetName());
            
            foreach (var dialogueChoice in m_Choices.Choices)
            {
                DialogueChoiceDisplay choiceDisplay = Instantiate(m_DialogueChoiceDisplayTemplate, m_ChoicesContainer);
                
                m_Views.Add(choiceDisplay);
                
                choiceDisplay.FeedData(dialogueChoice);
            }

            this.GetComponent<AccessibleUIGroupRoot>().RefreshNextUpdate();
        }

        protected override void OnHideStart()
        {
            base.OnHideStart();
            
            foreach (var dialogueChoiceDisplay in m_Views)
            {
                dialogueChoiceDisplay.GetComponentInChildren<Button>().interactable = false;
            }
        }

        protected override void OnHideCompleted()
        {
            base.OnHideCompleted();

            foreach (var dialogueChoiceDisplay in m_Views)
            {
                Destroy(dialogueChoiceDisplay.gameObject);
            }
            m_Views.Clear();
        }

        private void Update()
        {
            // TODO: Find another way to calculate this layout.
            // Force layout rebuild do display thoughts properly.
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_ChoicesContainer.GetComponent<RectTransform>());
        }

        private void OnChoiceSelected(DialogueChoice choice)
        {
            m_Selected = choice;
            
            Hide();
        }

        public bool IsChoiceIndex(int index) =>
            m_Selected != null && m_Choices != null && m_Choices.Choices.IndexOf(m_Selected) == index;
    }
}