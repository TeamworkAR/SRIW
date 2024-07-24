using Core;
using Data.CharacterData;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace UI.AlternativeDialogue
{
    public class AlternativeDialogueUI : BaseUICanvasGroup
    {
        [Header(Consts.EditorStrings.k_EDITOR_HEADERS_UIREFERENCES)]
        [SerializeField] private TextMeshProUGUI m_Title = null;
        [SerializeField] private TextMeshProUGUI m_Text = null;

        [SerializeField] private RawImage m_CharacterImage = null;
        
        private CharacterData m_CharacterData = null;
        
        public override bool IsDone()
        {
            return base.IsDone() && IsOnScreen() == false;
        }

        public void Show(CharacterData characterData, LocalizedString title, LocalizedString text)
        {
            m_CharacterData = characterData;
            m_Title.text = LocalizationManager.Instance.GetLocalizedValue(title);
            m_Text.text = LocalizationManager.Instance.GetLocalizedValue(text);

            base.Show();
        }

        protected override void OnShowStart()
        {
            base.OnShowStart();
            
            m_CharacterImage.enabled = true;
            m_CharacterImage.texture = m_CharacterData.ShowcaseTemplate
                .GetInstance(this, CharacterShowcase.CameraPositions.CloseUp).ImageTexture;
        }

        protected override void OnHideCompleted()
        {
            base.OnHideCompleted();
            
            CharacterShowcase.ClearByOwner(this);

            m_CharacterData = null;
        }
    }
}