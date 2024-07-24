using Animation;
using Core;
using Data.CharacterData;
using Data.ScenarioSettings;
using Managers;
using TMPro;
using UI.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Learnings
{
    public class CarouselPanel : LearningsUIPanel
    {
        [SerializeField] private RawImage m_Image = null;

        [SerializeField] private TextMeshProUGUI m_Title = null;
        [SerializeField] private TextMeshProUGUI m_Text = null;

        [SerializeField] private Button m_Button = null;

        [SerializeField] private CarouselCounterContainer m_CarouselCounterContainer = null;

        private ScenarioSettings.LearningsExtension m_Extension = null;

        private Helpers.UI.CyclingList<ScenarioSettings.LearningsExtension.LearningsEntry> m_Entries = null;
        
        public override void Show()
        {
            base.Show();
            
            m_Extension = GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.LearningsExtension>();

            m_Entries =
                new Helpers.UI.CyclingList<ScenarioSettings.LearningsExtension.LearningsEntry>(m_Extension.Entries);

            m_CarouselCounterContainer.Init(m_Entries.Count);
            
            ShowContent();
            
            m_Button.gameObject.SetActive(false);
        }

        public override void Hide()
        {
            m_CarouselCounterContainer.Dispose();

            m_Current = null;
            
            CharacterShowcase.ClearByOwner(this);
            
            base.Hide();
        }

        private CharacterData m_Current = null;
        
        private void ShowContent()
        {
            if (ReferenceEquals(m_Current, m_Entries.GetCurrent().Character) == false)
            {
                CharacterShowcase.ClearByOwner(this);
                
                m_Current = m_Entries.GetCurrent().Character;
                
                CharacterShowcase showcase = m_Entries.GetCurrent().Character.ShowcaseTemplate
                    .GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);

                showcase.GetComponent<LearningsAnimations>().HandleLearnings();
                
                m_Image.texture = showcase.ImageTexture;
            }
            
            m_Title.text = LocalizationManager.Instance.GetLocalizedValue(m_Entries.GetCurrent().Title);
            m_Text.text = LocalizationManager.Instance.GetLocalizedValue(m_Entries.GetCurrent().Content);
         
            m_CarouselCounterContainer.Select(m_Entries.Idx);
            
            m_Button.gameObject.SetActive(m_Entries.IsLast);
        }

        public void Next()
        {
            m_Entries.Next();
            
            ShowContent();
        }

        public void Previous()
        {
            m_Entries.Previous();
            
            ShowContent();
        }
    }
}