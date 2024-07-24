using UnityEngine;
using UnityEngine.UI;

namespace UI.Smartphone
{
    public class ImageSmartphoneChainEntryDisplay : MonoBehaviour
    {
        [SerializeField] private RawImage m_CharacterImage = null;

        [SerializeField] private Image m_Image = null;
        
        public void FeedData(ImageSmartPhoneChainEntry data)
        {
            m_CharacterImage.texture = data.Character.ShowcaseTemplate
                .GetInstance(this, CharacterShowcase.CameraPositions.CloseUp).ImageTexture;

            m_Image.sprite = data.Image;
        }

        private void OnDestroy()
        {
            CharacterShowcase.ClearByOwner(this);
        }
    }
}