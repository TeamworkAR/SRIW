using Core;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ContextSettings
{
    public class CharacterDisplay : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_CanvasGroup = null;
        
        [SerializeField] private RawImage m_Texture = null;

        private Coroutine m_Running = null;

        public bool IsAnimating => m_Running != null;
        
        public void Show(CharacterShowcase characterShowcase, bool useFadeIn = true)
        {
            m_Texture.texture = characterShowcase.ImageTexture;

            if (useFadeIn)
            {
                StartFadeIn();   
            }
            else
            {
                m_CanvasGroup.alpha = 1f;
            }
        }

        public void Hide(bool useFadeOut = true)
        {
            if (useFadeOut)
            {
                StartFadeOut();
            }
            else
            {
                m_CanvasGroup.alpha = 0f;
            }
        }

        public void StartFadeIn()
        {
            if (m_Running == null)
            {
                m_Running = StartCoroutine(Helpers.UI.COR_Fade(this.m_CanvasGroup, 0f, 1f,
                    GameManager.Instance.DevSettings.CharacterDisplayFadeDuration, () => m_Running = null));
            }
        }
        
        public void StartFadeOut()
        {
            if (m_Running == null)
            {
                m_Running = StartCoroutine(Helpers.UI.COR_Fade(this.m_CanvasGroup, 1f, 0f,
                    GameManager.Instance.DevSettings.CharacterDisplayFadeDuration, () => m_Running = null));
            }
        }
    }
}