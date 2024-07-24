using Animation;
using Data.CharacterData;
using Data.ScenarioSettings;
using Managers;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UI.Learnings.ReworkedLearningsUI;

namespace UI.Learnings
{
    public class LearningsDisplayPanel : ReworkedLearningsUIPanel 
    {
        [SerializeField] private RawImage m_CharacterImage = null;

        [SerializeField] private RectTransform m_ButtonsContainer = null;
        [SerializeField] private LearningsButton m_ButtonTemplate = null;

        [SerializeField] private TextMeshProUGUI m_Title = null;
        [SerializeField] private TextMeshProUGUI m_Text = null;

        [SerializeField] private Button m_Button = null;

        private ScenarioSettings.LearningsExtension m_Extension = null;

        private List<LearningsButton> m_Buttons = new List<LearningsButton>(0);

        public override void Show()
        {
            base.Show();

            m_Extension = GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.LearningsExtension>();

            foreach (ScenarioSettings.LearningsExtension.LearningsEntry entry in m_Extension.Entries)
            {
                LearningsButton button = Instantiate(m_ButtonTemplate, m_ButtonsContainer);

                m_Buttons.Add(button);

                button.Init(this, entry);
            }

            m_Buttons[0].ShowContent();
            m_Buttons[0].GetComponentInChildren<AccessibleButton>()?.Select();
            ExecuteEvents.Execute<ISelectHandler>(m_Buttons[0].gameObject, null, ExecuteEvents.selectHandler);
            EventSystem.current.SetSelectedGameObject(m_Buttons[0].gameObject);

            m_Button.gameObject.SetActive(false);
        }

        public override void Hide()
        {
            base.Hide();

            foreach (LearningsButton button in m_Buttons) 
            {
                Destroy(button.gameObject);
            }
            m_Buttons.Clear();

            m_Current = null;

            CharacterShowcase.ClearByOwner(this);
        }

        private CharacterData m_Current = null;

        public void ShowContent(ScenarioSettings.LearningsExtension.LearningsEntry entry)
        {
            if (ReferenceEquals(m_Current, entry.Character) == false)
            {
                CharacterShowcase.ClearByOwner(this);

                m_Current = entry.Character;

                CharacterShowcase showcase = entry.Character.ShowcaseTemplate
                    .GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);

                showcase.GetComponent<LearningsAnimations>().HandleLearnings();

                m_CharacterImage.texture = showcase.ImageTexture;
            }
            m_Text.text = LocalizationManager.Instance.GetLocalizedValue(entry.Content);
            m_Text.GetComponentInChildren<AccessibleLabel>()?.Select();
            m_Button.gameObject.SetActive(m_Extension.Entries.IndexOf(entry) == m_Extension.Entries.Count - 1);
        }
    }
}