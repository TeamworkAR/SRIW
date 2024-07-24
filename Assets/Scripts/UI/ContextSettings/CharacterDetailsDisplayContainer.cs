using System.Collections;
using Core;
using Data;
using Data.CharacterData;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ContextSettings
{
    public class CharacterDetailsDisplayContainer : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_CanvasGroup = null;

        [SerializeField] private RawImage m_Texture = null;
        [SerializeField] private TextMeshProUGUI m_TextName = null;
        [SerializeField] private TextMeshProUGUI m_TextDescription = null;
        
        private Coroutine m_Running = null;

        public bool IsAnimating => m_Running != null;
        
        public void Show(CharacterShowcase characterShowcase, CharacterData characterData)
        {
            this.gameObject.SetActive(true);
            m_Texture.texture = characterShowcase.ImageTexture;

            m_TextName.text = characterData.GetName();

            m_TextDescription.text = characterData.GetDescription();
        }

        public void Hide()
        {
            m_CanvasGroup.alpha = 0;

            m_Texture.texture = null;

            m_TextName.text = string.Empty;

            m_TextDescription.text = string.Empty;
            this.gameObject.SetActive(false);
        }

        public void StartFadeIn()
        {
            if (m_Running == null)
            {
                m_Running = StartCoroutine(COR_FadeIn());
            }
        }

        public void StopFadeIn()
        {
            if (m_Running != null)
            {
                StopCoroutine(m_Running);
                
                m_Running = null;
            }
            this.GetComponentInChildren<AccessibleLabel>().Select();
        }
        
        public void StartFadeOut()
        {
            if (m_Running == null)
            {
                m_Running = StartCoroutine(COR_FadeOut());
            }
        }

        public void StopFadeOut()
        {
            if (m_Running != null)
            {
                StopCoroutine(m_Running);
                
                m_Running = null;
            }
        }

        private IEnumerator COR_FadeIn()
        {
            yield return Helpers.UI.COR_Fade(this.m_CanvasGroup, 0f, 1f,
                GameManager.Instance.DevSettings.CharacterDisplayFadeDuration);
            
            StopFadeIn();
        }
        
        private IEnumerator COR_FadeOut()
        {
            yield return Helpers.UI.COR_Fade(this.m_CanvasGroup, 1f, 0f,
                GameManager.Instance.DevSettings.CharacterDisplayFadeDuration);
            
            StopFadeOut();
        }
    }
}