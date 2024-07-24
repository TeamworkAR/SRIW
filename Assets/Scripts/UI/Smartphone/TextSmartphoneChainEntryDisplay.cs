using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Smartphone
{
    public class TextSmartphoneChainEntryDisplay : MonoBehaviour
    {
        [SerializeField] private RawImage m_CharacterImage = null;

        [SerializeField] private TextMeshProUGUI m_Text = null;
        
        public void FeedData(TextSmartPhoneChainEntry data)
        {
            m_CharacterImage.texture = data.Character.ShowcaseTemplate
                .GetInstance(this, CharacterShowcase.CameraPositions.CloseUp).ImageTexture;

            m_Text.text = LocalizationManager.Instance.GetLocalizedValue(data.Text);
            m_Text.GetComponent<AccessibleLabel>()?.Select();
        }

        private void OnDestroy()
        {
            CharacterShowcase.ClearByOwner(this);
        }
    }
}