using System;
using System.Collections.Generic;
using Core;
using Data.CharacterData;
using Managers;
using NodeEditor.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace UI.MoodCheckIn
{
    public class MoodCheckInUI : BaseUICanvasGroup
    {
        [SerializeField] private MoodCheckInDisplay m_Template = null;

        [SerializeField] private Transform m_MoodCheckInDisplayContainer = null;

        [SerializeField] private TextMeshProUGUI m_TitleText = null;

        [SerializeField] private LocalizedString m_LocalizedString = null;
        
        private Coroutine m_Running = null;

        private List<MoodCheckInDisplay> m_MoodCheckInDisplays = new List<MoodCheckInDisplay>(0);

        private const float k_CHOSEN_WAIT_TIME = 4f;

        private bool b_MoodChosen = false;

        public override bool IsDone() => base.IsDone() && m_Running == null && b_MoodChosen;

        private MoodCheckInEntry m_Entry = null;

        private CharacterData m_CharacterData = null;

        public void Show(MoodCheckInEntry entry)
        {
            m_Entry = entry;
            
            base.Show();
        }
        
        protected override void OnShowStart()
        {
            base.OnShowStart();
            
            b_MoodChosen = false;
            
            foreach (var mood in m_Entry.Moods)
            {
                CharacterShowcase showcase =
                    m_Entry.Character.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);
                
                MoodCheckInDisplay moodCheckInDisplay = GameObject.Instantiate(m_Template, m_MoodCheckInDisplayContainer);
                m_MoodCheckInDisplays.Add(moodCheckInDisplay);
                
                moodCheckInDisplay.Show(showcase, mood);
                
                moodCheckInDisplay.OnMoodChosen += OnMoodChosen;
            }

            m_TitleText.text = string.Format(LocalizationManager.Instance.GetLocalizedValue(m_LocalizedString), m_Entry.Character.GetName());
        }

        protected override void OnHideStart()
        {
            base.OnHideStart();
            
            if (m_Running != null)
            {
                StopCoroutine(m_Running);

                m_Running = null;
            }
        }

        protected override void OnHideCompleted()
        {
            base.OnHideCompleted();
            
            foreach (var moodCheckInDisplay in m_MoodCheckInDisplays)
            {
                moodCheckInDisplay.OnMoodChosen -= OnMoodChosen;
                Destroy(moodCheckInDisplay.gameObject);
            }
            
            m_Running = null;
            
            m_MoodCheckInDisplays.Clear();

            CharacterShowcase.ClearByOwner(this);
        }

        private void OnMoodChosen()
        {
            foreach (var moodCheckInDisplay in m_MoodCheckInDisplays)
            {
                moodCheckInDisplay.DisableInteraction();
            }

            b_MoodChosen = true;
            
            Hide();

            m_Running = null;
        }

        [Serializable]
        public class Mood
        {
            [SerializeField] private Consts.Moods.Mood m_FacialExpressionsMood = Consts.Moods.Mood.None;
            
            [SerializeField] private AnimatorOverrideController m_AnimatorControllerOverride = null;
            
            [SerializeField] private LocalizedString m_Name = default;

            public AnimatorOverrideController ControllerOverride => m_AnimatorControllerOverride;

            public string Name => LocalizationManager.Instance.GetLocalizedValue(m_Name);

            public Consts.Moods.Mood FacialExpressionsMood => m_FacialExpressionsMood;
        }
    }
}