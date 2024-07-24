using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animation;
using Core;
using Data.CharacterData;
using Data.ScenarioSettings;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ContextSettings
{
    public class ContextSettingsUI : BaseUICanvasGroup
    {
        [SerializeField] private LayoutGroup m_RightGroupCharacterDisplayContainer = null;
        [SerializeField] private LayoutGroup m_LeftGroupCharacterDisplayContainer = null;
        
        [SerializeField] private CanvasGroup m_OuterCircleLayoutGroup = null;

        [SerializeField] private CanvasGroup m_CharacterGroupDisplayLayoutGroup = null;
        
        [SerializeField] private CharacterDetailsDisplayContainer m_LeftCharacterDetailsDisplayContainer = null;
        [SerializeField] private CharacterDetailsDisplayContainer m_RightCharacterDetailsDisplayContainer = null;
        
        [SerializeField] private CharacterDisplay m_Template_Left = null;
        [SerializeField] private CharacterDisplay m_Template_Right = null;

        [SerializeField] private TextMeshProUGUI m_GroupDescription = null;

        private Dictionary<LayoutGroup, List<(CharacterDisplay characterDisplay, CharacterShowcase showcase,
            CharacterData characterData)>> m_CharacterDisplayInstances =
            new Dictionary<LayoutGroup, List<(CharacterDisplay characterDisplay, CharacterShowcase showcase,
                CharacterData characterData)>>(0);

        private Dictionary<LayoutGroup, CharacterDisplay> m_CharacterDisplayTemplateMap = null;
        
        public override bool IsDone() => base.IsDone() && m_Running == null;

        private Coroutine m_Running = null;

        private ScenarioSettings.ContextSettingsExtension m_Data = null;

        protected override void Awake()
        {
            base.Awake();

            m_CharacterDisplayTemplateMap = new Dictionary<LayoutGroup, CharacterDisplay>()
            {
                { m_LeftGroupCharacterDisplayContainer, m_Template_Left },
                { m_RightGroupCharacterDisplayContainer, m_Template_Right }
            };
        }

        protected override void OnShowStart()
        {
            base.OnShowStart();
            
            m_Data = GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.ContextSettingsExtension>();

            SpawnAtGroup(m_LeftGroupCharacterDisplayContainer, m_Data.LeftCharacters);
            SpawnAtGroup(m_RightGroupCharacterDisplayContainer, m_Data.RightCharacters);
            
            m_OuterCircleLayoutGroup.alpha = 0f;
            
            foreach (var valueTuples in m_CharacterDisplayInstances.Values)
            {
                foreach (var valueTuple in valueTuples)
                {
                    valueTuple.characterDisplay.Show(valueTuple.showcase, false);
                    valueTuple.characterDisplay.Hide(false);
                }
            }
        }

        protected override void OnShowCompleted()
        {
            base.OnShowCompleted();
            
            StartLifeCycle();
        }

        protected override void OnHideCompleted()
        {
            base.OnHideCompleted();
            
            StopLifeCycle();
            
            foreach (var valueTuples in m_CharacterDisplayInstances.Values)
            {
                foreach (var valueTuple in valueTuples)
                {
                    Destroy(valueTuple.characterDisplay.gameObject);
                }
            }
            m_CharacterDisplayInstances.Clear();
            
            m_LeftCharacterDetailsDisplayContainer.Hide();
            m_RightCharacterDetailsDisplayContainer.Hide();
            
            CharacterShowcase.ClearByOwner(this);

            m_OuterCircleLayoutGroup.alpha = 1f;

            m_CharacterGroupDisplayLayoutGroup.alpha = 0f;
            m_CharacterGroupDisplayLayoutGroup.gameObject.SetActive(false);
        }
        
        private void SpawnAtGroup(LayoutGroup layoutGroup, List<CharacterData> characters)
        {
            foreach (var characterData in characters)
            {
                CharacterDisplay characterDisplay = Instantiate(m_CharacterDisplayTemplateMap[layoutGroup], layoutGroup.transform);

                CharacterShowcase characterShowcase =
                    characterData.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.HalfBody);
                
                characterShowcase.GetComponentInChildren<ContextSettingsAnimations>().HandleContextSettings();

                if (m_CharacterDisplayInstances.ContainsKey(layoutGroup) == false)
                {
                    m_CharacterDisplayInstances.Add(layoutGroup,
                        new List<(CharacterDisplay characterDisplay, CharacterShowcase showcase, CharacterData characterData)>(0));
                }

                m_CharacterDisplayInstances[layoutGroup].Add((characterDisplay, characterShowcase, characterData));
            }
        }

        private void StartLifeCycle()
        {
            if (m_Running == null)
            {
                m_Running = StartCoroutine(COR_LifeCycle());   
            }
            else
            {
                // TODO: Error message
                Debug.LogError("");
            }    
        }

        private void StopLifeCycle()
        {
            if (m_Running != null)
            {
                StopCoroutine(m_Running);
                m_Running = null;
            }
            else
            {
                // TODO: Error message
                Debug.LogError("");
            }
        }

        private IEnumerator COR_LifeCycle()
        {
            // m_OuterCircleLayoutGroup.alpha = 0f;
            
            // foreach (var valueTuples in m_CharacterDisplayInstances.Values)
            // {
            //     foreach (var valueTuple in valueTuples)
            //     {
            //         valueTuple.characterDisplay.Show(valueTuple.showcase, false);
            //         valueTuple.characterDisplay.Hide(false);
            //     }
            // }
            //
            // while (m_CharacterDisplayInstances.Values.SelectMany( kvp => kvp).Any( kvp => kvp.characterDisplay.IsAnimating))
            // {
            //     yield return null;
            // }
            
            // yield return Helpers.UI.COR_Fade(m_OuterCircleLayoutGroup, 1f, 0f, 1f);
            
            for (int i = 0; i < Mathf.Max(m_CharacterDisplayInstances.Values.Select(l => l.Count).ToArray()); i++)
            {
                CharacterDetailsDisplayContainer previousDisplayContainer = null;
                foreach (var valueTuples in m_CharacterDisplayInstances.Values)
                {
                    if (i < valueTuples.Count)
                    {
                        // valueTuples[i].characterDisplay.StartFadeOut();
                        //
                        // while (valueTuples[i].characterDisplay.IsAnimating)
                        // {
                        //     yield return null;
                        // }

                        CharacterShowcase showcase = valueTuples[i].characterData.ShowcaseTemplate
                            .GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);
                        
                        showcase.GetComponentInChildren<ContextSettingsAnimations>().HandleContextSettings();

                        // TODO: Refactor, take into consideration a specialized class
                        CharacterDetailsDisplayContainer detailsDisplayContainer = null;
                        foreach (var layoutGroup in m_CharacterDisplayInstances.Keys)
                        {
                            if (ReferenceEquals(m_CharacterDisplayInstances[layoutGroup], valueTuples))
                            {
                                detailsDisplayContainer =
                                    ReferenceEquals(layoutGroup, m_LeftGroupCharacterDisplayContainer)
                                        ? m_LeftCharacterDetailsDisplayContainer
                                        : m_RightCharacterDetailsDisplayContainer;
                                
                                break;
                            }
                        }

                        detailsDisplayContainer.Show(showcase, valueTuples[i].characterData);
                        
                        detailsDisplayContainer.StartFadeIn();

                        while (detailsDisplayContainer.IsAnimating)
                        {
                            yield return null;
                        }
                        
                        //Hide the previous container so it disables it
                        previousDisplayContainer?.Hide();
                        previousDisplayContainer = detailsDisplayContainer;
                        
                        // TODO: Very stubby way to handle user read time. Also, CharacterDisplay should take care to yield this time
                        yield return new WaitForSeconds(Helpers.UI.GetReadTime(valueTuples[i].characterData.GetDescription()));
                        
                        detailsDisplayContainer.StartFadeOut();
                        
                        while (detailsDisplayContainer.IsAnimating)
                        {
                            yield return null;
                        }
                        
                        // valueTuples[i].characterDisplay.StartFadeIn();
                        //
                        // while (valueTuples[i].characterDisplay.IsAnimating)
                        // {
                        //     yield return null;
                        // }
                    }
                }
            }

            foreach (var valueTuples in m_CharacterDisplayInstances.Values)
            {
                foreach (var valueTuple in valueTuples)
                {
                    valueTuple.characterDisplay.StartFadeIn();
                }
            }
            
            m_GroupDescription.text = m_Data.RecapText;
            m_CharacterGroupDisplayLayoutGroup.gameObject.SetActive(true);
            yield return Helpers.UI.COR_Fade(m_CharacterGroupDisplayLayoutGroup, 0f,
                GameManager.Instance.DevSettings.CharacterDisplayFadeDuration, 1f);
            m_GroupDescription.GetComponent<AccessibleLabel>().Select();
            yield return new WaitForSeconds(Helpers.UI.GetReadTime(m_GroupDescription.text));
            
            Hide();
        }
    }
}