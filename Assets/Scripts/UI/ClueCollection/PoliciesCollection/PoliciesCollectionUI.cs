using System;
using System.Collections;
using System.Collections.Generic;
using Animation;
using Core;
using Data.ScenarioSettings;
using Managers;
using TMPro;
using UI.ClueCollection.ClueBook;
using UI.ContextSettings;
using UI.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ClueCollection.PoliciesCollection
{
    public sealed class PoliciesCollectionUI : BaseUICanvas
    {
        [SerializeField] private TextMeshProUGUI m_Text = null;
        [SerializeField] private TextMeshProUGUI m_Title = null;
        
        [SerializeField] private Transform m_CharactersLeftGroupContainer = null;
        [SerializeField] private Transform m_CharactersRightGroupContainer = null;

        [SerializeField] private CharacterDisplay m_LeftCharacterDisplayTemplate = null;
        [SerializeField] private CharacterDisplay m_RightCharacterDisplayTemplate = null;

        [SerializeField] private Image m_CircleFill = null;

        [SerializeField] private GameObject m_CheckMarkIcon = null;
        [SerializeField] private GameObject m_NextUIButton = null;

        [SerializeField] private RectTransform m_TitleContainer = null;

        [SerializeField] private CarouselCounterContainer m_CarouselCounterContainer = null;
        
        private ScenarioSettings.ClueCollectionExtension m_Extension = null;

        private List<CharacterDisplay> m_CharacterDisplayInstances = new List<CharacterDisplay>(0);

        private Helpers.UI.CyclingList<ScenarioSettings.ClueCollectionExtension.Policy> m_Content = null;

        private Coroutine m_Running = null;

        private void Start()
        {
            m_CircleFill.color = GameManager.Instance.DevSettings.UIGreen;
        }

        public override void Show()
        {
            base.Show();

            ClueBookUI.OnClueBookShow += OnClueBookShow;
            ClueBookUI.OnClueBookHide += OnClueBookHide;
            
            m_Extension = GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.ClueCollectionExtension>();
            
            foreach (var rightGroupCharacter in m_Extension.RightGroupCharacters)
            {
                CharacterDisplay characterDisplay = Instantiate(m_RightCharacterDisplayTemplate, m_CharactersRightGroupContainer);
                
                m_CharacterDisplayInstances.Add(characterDisplay);

                CharacterShowcase characterShowcase = rightGroupCharacter.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.FullBody);

                characterShowcase.GetComponent<PolicyCollectionAnimations>().HandlePolicyCollection();

                characterDisplay.Show(characterShowcase, false);
            }
            
            foreach (var leftGroupCharacter in m_Extension.LeftGroupCharacters)
            {
                CharacterDisplay characterDisplay = Instantiate(m_LeftCharacterDisplayTemplate, m_CharactersLeftGroupContainer);
                
                m_CharacterDisplayInstances.Add(characterDisplay);
                
                CharacterShowcase characterShowcase = leftGroupCharacter.ShowcaseTemplate.GetInstance(this,CharacterShowcase.CameraPositions.FullBody);

                characterShowcase.GetComponent<PolicyCollectionAnimations>().HandlePolicyCollection();

                characterDisplay.Show(characterShowcase, false);
            }

            m_Content = new Helpers.UI.CyclingList<ScenarioSettings.ClueCollectionExtension.Policy>(m_Extension.GetPolicies());

            m_CarouselCounterContainer.Init(m_Content.Count);

            m_NextUIButton.SetActive(false);
            
            ShowContent();
        }
        
        public override void Hide()
        {
            base.Hide();

            ClueBookUI.OnClueBookShow -= OnClueBookShow;
            ClueBookUI.OnClueBookHide -= OnClueBookHide;
            
            foreach (var characterDisplayInstance in m_CharacterDisplayInstances)
            {
                Destroy(characterDisplayInstance.gameObject);
            }
            m_CharacterDisplayInstances.Clear();
            
            m_CarouselCounterContainer.Dispose();
            
            TryStopLifecycle();
            
            CharacterShowcase.ClearByOwner(this);
        }

        private void OnClueBookHide()
        {
            ShowContent();
        }

        private void OnClueBookShow()
        {
            TryStopLifecycle();
        }

        public void Next()
        {
            m_Content.Next(false);
            
            ShowContent();
        }

        public void Previous()
        {
            m_Content.Previous(false);
            
            ShowContent();
        }

        private void ShowContent()
        {
            m_CarouselCounterContainer.EnableInteraction(m_Content.Idx);
            m_CarouselCounterContainer.Select(m_Content.Idx);
            
            m_Text.text = m_Content.GetCurrent().GetText();
            m_Title.text = m_Content.GetCurrent().GetTitle();

            if (m_Content.GetCurrent().IsUnlocked() == false)
            {
                m_CheckMarkIcon.SetActive(false);
                
                TryStartLifecycle();   
            }
            else
            {
                m_CheckMarkIcon.SetActive(true);
                
                m_NextUIButton.SetActive(m_Content.IsLast && m_Content.GetCurrent().IsUnlocked());
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_TitleContainer);
        }

        private void TryStartLifecycle()
        {
            if (m_Running != null)
            {
                return;
            }
            
            m_Running = StartCoroutine(COR_PolicyLifeCycle());
        }

        private void TryStopLifecycle()
        {
            if (m_Running == null)
            {
                return;
            }
            
            StopCoroutine(m_Running);

            m_Running = null;
        }

        private IEnumerator COR_PolicyLifeCycle()
        {
            m_CarouselCounterContainer.DisableInteraction();
            
            yield return Helpers.UI.COR_FillImage(m_CircleFill, 0f, 1f, GameManager.Instance.DevSettings.PoliciesFixedReadTime);
            
            m_CheckMarkIcon.gameObject.SetActive(true);
            
            yield return Helpers.UI.COR_Scale(m_CheckMarkIcon.transform, Vector3.zero, Vector3.one, 0.5f);

            m_Content.GetCurrent().Unlock();
            
            m_CarouselCounterContainer.EnableInteraction(m_Content.Idx);
            
            m_NextUIButton.SetActive(m_Content.IsLast && m_Content.GetCurrent().IsUnlocked());
            
            TryStopLifecycle();
        }
    }
}