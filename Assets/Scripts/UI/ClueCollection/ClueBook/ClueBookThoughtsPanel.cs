using System.Collections;
using System.Collections.Generic;
using Animation;
using Data.CharacterData;
using Data.ScenarioSettings;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace UI.ClueCollection.ClueBook
{
    public class ClueBookThoughtsPanel : ClueBookUI.ClueBookPanel
    {
        [SerializeField] private RawImage m_CharacterImage = null;
        
        [SerializeField] private RectTransform m_CharactersButtonContainer = null;

        [SerializeField] private ClueBookCharacterButton m_CharacterButtonTemplate = null;

        [SerializeField] private RectTransform m_ThoughtsContainer = null;

        [SerializeField] private RectTransform m_LeftTitleContainer = null;
        
        [SerializeField] private ClueBookThoughtDisplay m_ThooughtDisplayTemplate = null;

        [SerializeField] private ClueBookTabButton m_TabButton = null;

        [SerializeField] private Scrollbar m_Scrollbar = null;

        [SerializeField] private ScrollRect m_Scrollrect = null;

        [SerializeField] private Image m_ScrollViewFadeImage = null;

        [SerializeField] private GameObject m_LockedViz = null;

        [SerializeField] private LocalizedString m_ThoughtsTitle = null;

        [SerializeField] private TextMeshProUGUI m_ThoughtsTitleText = null;

        private ScenarioSettings.ClueCollectionExtension m_Extension = null;

        private List<ClueBookCharacterButton> m_Buttons = new List<ClueBookCharacterButton>(0);

        private List<ClueBookThoughtDisplay> m_ThoughtDisplays = new List<ClueBookThoughtDisplay>(0);

        private Coroutine m_Running = null;
        
        public override void Show()
        {
            base.Show();

            m_Extension = GameManager.Instance.ScenarioSettings
                .GetExtension<ScenarioSettings.ClueCollectionExtension>();
            
            // Handle top right button?
            
            // TODO: Extract method
            foreach (var leftGroupCharacter in m_Extension.LeftGroupCharacters)
            {
                ClueBookCharacterButton button = Instantiate(m_CharacterButtonTemplate, m_CharactersButtonContainer);

                m_Buttons.Add(button);
                
                button.FeedData(leftGroupCharacter, this);
            }
            foreach (var rightGroupCharacter in m_Extension.RightGroupCharacters)
            {
                ClueBookCharacterButton button = Instantiate(m_CharacterButtonTemplate, m_CharactersButtonContainer);
                
                m_Buttons.Add(button);
                
                button.FeedData(rightGroupCharacter, this);
            }
            
            // Show for first character
            m_Buttons[0].SelectCharacter();

            m_TabButton.SetActiveViz();

            m_Scrollbar.onValueChanged.AddListener(OnValueChanged);

            m_Running = StartCoroutine(COR_Update());
        }
        
        public override void Hide()
        {
            base.Hide();

            Clear();
            
            m_TabButton.SetInactiveViz();

            m_Scrollbar.onValueChanged.RemoveListener(OnValueChanged);

            StopCoroutine(m_Running);
            m_Running = null;
        }
        
        public void ShowForCharacter(CharacterData characterData)
        {
            CharacterShowcase.ClearByOwner(this);
            // Clear thoughts
            for (int i = 0; i < m_ThoughtDisplays.Count; i++)
            {
                Destroy(m_ThoughtDisplays[i].gameObject);
            }
            m_ThoughtDisplays.Clear();

            CharacterShowcase characterShowcase =
                characterData.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);
            
            characterShowcase.GetComponent<ThoughtCollectionAnimations>().HandleThoughtCollection();
            
            m_CharacterImage.texture = characterShowcase.ImageTexture;

            var thoughts = m_Extension.GetThoughtsFor(characterData, true);

            m_LockedViz.SetActive(thoughts.Exists(t => t.IsUnlocked()) == false);

            if (thoughts.Count > 0)
            {
                // Spawn thoughts
                foreach (var thought in thoughts)
                {
                    if (thought.IsUnlocked() == false)
                    {
                        continue;
                    }

                    ClueBookThoughtDisplay thoughtDisplay = Instantiate(m_ThooughtDisplayTemplate, m_ThoughtsContainer);

                    m_ThoughtDisplays.Add(thoughtDisplay);

                    thoughtDisplay.FeedData(thought);
                }
            }

            m_ThoughtsTitleText.text = string.Format(LocalizationManager.Instance.GetLocalizedValue(m_ThoughtsTitle), characterData.GetName());  
        }

        private void Clear()
        {
            // Clear character image
            CharacterShowcase.ClearByOwner(this);
            
            // Clear thoughts
            for (int i = 0; i < m_ThoughtDisplays.Count; i++)
            {
                Destroy(m_ThoughtDisplays[i].gameObject);
            }
            m_ThoughtDisplays.Clear();
            
            for (int i = 0; i < m_Buttons.Count; i++)
            {
                Destroy(m_Buttons[i].gameObject);
            }
            m_Buttons.Clear();
        }

        private void OnValueChanged(float value)
        {
            m_ScrollViewFadeImage.color = new Color(m_ScrollViewFadeImage.color.r, m_ScrollViewFadeImage.color.g, m_ScrollViewFadeImage.color.b, value);
        }

        private IEnumerator COR_Update()
        {
            while (true)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(m_ThoughtsContainer);

                LayoutRebuilder.ForceRebuildLayoutImmediate(m_LeftTitleContainer);

                m_ScrollViewFadeImage.gameObject.SetActive(m_Scrollrect.content.sizeDelta.y > m_Scrollrect.viewport.sizeDelta.y);

                yield return null;
            }
        }
    }
}