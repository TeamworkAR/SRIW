using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animation;
using Data.CharacterData;
using Data.ScenarioSettings;
using Managers;
using TMPro;
using UI.ClueCollection.ClueBook;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace UI.ClueCollection.ThoughtsCollection
{
    public class ThoughtsDisplayPanel : BaseUICanvas
    {
        [SerializeField] private RawImage m_Image = null;

        [SerializeField] private GameObject m_ThoughtsContainer = null;
        
        [SerializeField] private GameObject m_NextButtonContainer = null;

        [SerializeField] private GameObject m_HandPointing = null;
        
        [SerializeField] private ThoughtDisplay m_ThoughtDisplayTemplate = null;

        [SerializeField] private TextMeshProUGUI m_TitleText = null;

        [SerializeField] private LocalizedString m_TitleLocalizedString = null;

        [SerializeField] private TextMeshProUGUI m_EndText = null;

        [SerializeField] private LocalizedString m_EndLocalizedString = null;

        [SerializeField] private AccessibleLabel m_ThoughtsLabel;

        private List<ThoughtDisplay> m_ThoughtDisplayInstances = new List<ThoughtDisplay>(0);
        
        private CharacterData m_CharacterData = null;

        private Coroutine m_Running = null;

        private ScenarioSettings.ClueCollectionExtension m_Extension = null;

        private bool b_NextPressed = false;
        
        public bool ThoughtsCollected => b_NextPressed && m_Running == null;

        public void ShowStandalone(PortableThoughtsWrapper wrapper)
        {
            base.Show();

            m_CharacterData = wrapper.Wrapper.Owner;
            
            b_NextPressed = false;
            
            ClueBookUI.OnClueBookShow += OnClueBookShow;
            ClueBookUI.OnClueBookHide += OnClueBookHide;
            
            m_ThoughtsContainer.SetActive(true);
            m_NextButtonContainer.SetActive(false);
            m_HandPointing.SetActive(false);

            CharacterShowcase characterShowcase = m_CharacterData.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);

            characterShowcase.GetComponent<ThoughtCollectionAnimations>().HandleThoughtCollection();

            m_Image.texture = characterShowcase.ImageTexture;
            
            foreach (var thought in wrapper.Wrapper.Thoughts)
            {
                ThoughtDisplay thoughtDisplay = Instantiate(m_ThoughtDisplayTemplate, m_ThoughtsContainer.transform);
                
                m_ThoughtDisplayInstances.Add(thoughtDisplay);

                thoughtDisplay.FeedData(thought);
            }

            m_TitleText.text = string.Format(LocalizationManager.Instance.GetLocalizedValue(m_TitleLocalizedString), m_CharacterData.GetName());
            
            var aGroup = this.GetComponent<AccessibleUIGroupRoot>();
            if (aGroup != null)
            {
                aGroup.enabled = true;
            }
            
            TryStartLifeCycle();
        }

        public override void Show()
        {
            base.Show();

            b_NextPressed = false;
            
            ClueBookUI.OnClueBookShow += OnClueBookShow;
            ClueBookUI.OnClueBookHide += OnClueBookHide;
            
            m_ThoughtsContainer.SetActive(true);
            m_NextButtonContainer.SetActive(false);
            m_HandPointing.SetActive(false);

            CharacterShowcase characterShowcase = m_CharacterData.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);

            characterShowcase.GetComponent<ThoughtCollectionAnimations>().HandleThoughtCollection();

            m_Image.texture = characterShowcase.ImageTexture;

            m_Extension = GameManager.Instance.ScenarioSettings
                .GetExtension<ScenarioSettings.ClueCollectionExtension>();
            
            foreach (var thought in m_Extension.GetThoughtsFor(m_CharacterData))
            {
                ThoughtDisplay thoughtDisplay = Instantiate(m_ThoughtDisplayTemplate, m_ThoughtsContainer.transform);
                
                m_ThoughtDisplayInstances.Add(thoughtDisplay);

                thoughtDisplay.FeedData(thought);
            }

            m_TitleText.text = string.Format(LocalizationManager.Instance.GetLocalizedValue(m_TitleLocalizedString), m_CharacterData.GetName());

            var aGroup = this.GetComponent<AccessibleUIGroupRoot>();
            if (aGroup != null)
            {
                aGroup.enabled = true;
            }
            
            TryStartLifeCycle();
        }

        public override void Hide()
        {
            base.Hide();
            
            AudioManager.Instance.StopThoughts();
            
            ClueBookUI.OnClueBookShow -= OnClueBookShow;
            ClueBookUI.OnClueBookHide -= OnClueBookHide;
            
            m_ThoughtsContainer.SetActive(false);
            //m_NextButtonContainer.SetActive(false);
            
            CharacterShowcase.ClearByOwner(this);

            foreach (var thoughtDisplayInstance in m_ThoughtDisplayInstances)
            {
                thoughtDisplayInstance.StopAllCoroutines();
                Destroy(thoughtDisplayInstance.gameObject);    
            }
            m_ThoughtDisplayInstances.Clear();

            var aGroup = this.GetComponent<AccessibleUIGroupRoot>();
            if (aGroup != null)
            {
                aGroup.enabled = false;
            }
            
            TryStopLifeCycle();
        }

        private void TryStartLifeCycle()
        {
            if (m_Running != null)
            {
                return;
            }

            m_Running = StartCoroutine(COR_LifeCycle());
        }

        private void TryStopLifeCycle()
        {
            if (m_Running == null)
            {
                return;
            }
            
            StopCoroutine(m_Running);
            m_Running = null;
        }

        private IEnumerator COR_LifeCycle()
        {
            // Waiting one frame to proper init ThoughtDisplay(s)
            // I don't know why it's needed but it is needed indeed
            yield return null;
            
            m_ThoughtsLabel.Select();
            
            RectTransform thoughtsContainerTransform = m_ThoughtsContainer.GetComponent<RectTransform>();
            
            foreach (var thoughtDisplayInstance in m_ThoughtDisplayInstances)
            {
                thoughtDisplayInstance.ReadThought();

                while (thoughtDisplayInstance.IsThoughtPlaying())
                {
                    // TODO: Find another way to calculate this layout. This was previously in the Update function, placing this call here should improve performance overall.
                    // Force layout rebuild do display thoughts properly.
                    LayoutRebuilder.ForceRebuildLayoutImmediate(thoughtsContainerTransform);
                
                    yield return null;    
                }
            }

            m_ThoughtsContainer.SetActive(false);

            m_EndText.text = string.Format(LocalizationManager.Instance.GetLocalizedValue(m_EndLocalizedString), m_CharacterData.GetName());

            m_NextButtonContainer.SetActive(true);
            m_HandPointing.SetActive(true);
        }
        
        public void SelectCharacter(CharacterData data)
        {
            // TODO: We should differentiate this check for ClueCollection phase and stand alone ThoughtsCollection.
            if (GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.ClueCollectionExtension>().GetThoughtsFor(data, true).Count(t => t.IsUnlocked() == false) == 0)
            {
                return;
            }

            m_CharacterData = data;
            
            Show();
        }
        
        private void OnClueBookHide()
        {
            TryStartLifeCycle();

            m_HandPointing.SetActive(false);
        }

        private void OnClueBookShow()
        {
            TryStopLifeCycle();

            foreach (var thoughtDisplayInstance in m_ThoughtDisplayInstances)
            {
                thoughtDisplayInstance.Interrupt();
            }

            m_HandPointing.SetActive(false);
        }

        public void PressNext()
        {
            b_NextPressed = true;
            
            TryStopLifeCycle();
        }
    }
}