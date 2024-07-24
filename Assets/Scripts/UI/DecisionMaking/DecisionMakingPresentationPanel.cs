using Animation;
using Data.ScenarioSettings;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace UI.DecisionMaking
{
    public class DecisionMakingPresentationPanel : BaseUICanvas
    {
        [SerializeField] private RawImage m_CharacterImage = null;

        [SerializeField] private TextMeshProUGUI m_SubtitleText = null;

        [SerializeField] private LocalizedString m_SubtitleLocalizedString = null;
        
        public override void Show()
        {
            var extension =
                GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.DecisionMakingExtension>();

            m_SubtitleText.text = string.Format(LocalizationManager.Instance.GetLocalizedValue(m_SubtitleLocalizedString),
                extension.Chatacter.GetName());

            CharacterShowcase characterShowcase = extension.Chatacter.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);

            characterShowcase.GetComponent<DecisionMakingAnimations>().HandleDecisionMaking();

            m_CharacterImage.texture = characterShowcase.ImageTexture;

            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
            
            CharacterShowcase.ClearByOwner(this);
        }
    }
}