using System;
using System.Collections;
using System.Collections.Generic;
using Animation;
using Core;
using Data.CharacterData;
using Managers;
using TMPro;
using UI.ContextSettings;
using UnityEngine;
using UnityEngine.Localization;

namespace UI.PostDialogueCarousel
{
    public class PostDialogueCarouselUI : BaseUICanvasGroup
    {
        [Header(Consts.EditorStrings.k_EDITOR_HEADERS_UIREFERENCES)] 
        [SerializeField] private RectTransform m_LeftCharactersContainer = null;
        [SerializeField] private RectTransform m_RightCharactersContainer = null;
        [SerializeField] private TextMeshProUGUI m_Text = null;

        [Header(Consts.EditorStrings.k_EDITOR_HEADERS_TEMPLATES)] 
        [SerializeField] private CharacterDisplay m_LeftCharacterDisplayTemplate = null;
        [SerializeField] private CharacterDisplay m_RightCharacterDisplayTemplate = null;

        private PostDialogueCarouselData m_Data = null;

        private List<CharacterDisplay> m_CharacterDisplayInstances = new List<CharacterDisplay>(0);

        private Coroutine m_LifeCycle = null;
        
        public override bool IsDone()
        {
            return base.IsDone() && IsOnScreen() == false;
        }

        public void Show(PostDialogueCarouselData data)
        {
            m_Data = data;

            m_Text.text = null; // to ensure previous texts don't appear for a brief moment
            
            base.Show();    
        }

        protected override void OnShowStart()
        {
            foreach (var leftCharacter in m_Data.LeftCharacters)
            {
                CharacterDisplay characterDisplay = Instantiate(m_LeftCharacterDisplayTemplate, m_LeftCharactersContainer);

                CharacterShowcase showcase = leftCharacter.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.HalfBody);

                characterDisplay.Show(showcase, false);

                showcase.GetComponent<LearningsAnimations>().HandleLearnings();

                m_CharacterDisplayInstances.Add(characterDisplay);
            }
            
            foreach (var rightCharacter in m_Data.RightCharacters)
            {
                CharacterDisplay characterDisplay = Instantiate(m_RightCharacterDisplayTemplate, m_RightCharactersContainer);

                CharacterShowcase showcase = rightCharacter.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.HalfBody);

                characterDisplay.Show(showcase, false);

                showcase.GetComponent<LearningsAnimations>().HandleLearnings();

                m_CharacterDisplayInstances.Add(characterDisplay);
            }
            
            base.OnShowStart();
        }

        protected override void OnShowCompleted()
        {
            m_LifeCycle = StartCoroutine(COR_LifeCycle());
            
            base.OnShowCompleted();
        }

        protected override void OnHideCompleted()
        {
            CharacterShowcase.ClearByOwner(this);
            
            foreach (var characterDisplayInstance in m_CharacterDisplayInstances)
            {
                Destroy(characterDisplayInstance.gameObject);   
            }
            m_CharacterDisplayInstances.Clear();

            m_Data = null;

            if (m_LifeCycle != null)
            {
                StopCoroutine(m_LifeCycle);
                m_LifeCycle = null;
            }

            base.OnHideCompleted();
        }

        private IEnumerator COR_LifeCycle()
        {
            foreach (var localizedString in m_Data.Texts)
            {
                m_Text.text = LocalizationManager.Instance.GetLocalizedValue(localizedString);
                m_Text.gameObject.GetComponent<AccessibleLabel>().Select();

                yield return Helpers.UI.COR_Cooldown(Helpers.UI.GetReadTime(m_Text.text));
            }

            m_LifeCycle = null;

            Hide();
        }

        [Serializable]
        public class PostDialogueCarouselData
        {
            [SerializeField] private List<LocalizedString> m_Texts = new List<LocalizedString>(0);
            
            [SerializeField] private List<CharacterData> m_LeftCharacters = new List<CharacterData>(0);
            
            [SerializeField] private List<CharacterData> m_RightCharacters = new List<CharacterData>(0);

            public List<LocalizedString> Texts => m_Texts;

            public List<CharacterData> LeftCharacters => m_LeftCharacters;

            public List<CharacterData> RightCharacters => m_RightCharacters;
        }
    }
}