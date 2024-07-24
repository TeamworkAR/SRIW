using Data.ScenarioSettings;
using Managers;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Learnings
{
    public class LearningsButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private TextMeshProUGUI[] m_Texts = null;

        [SerializeField] private GameObject m_SelectedState = null;
        [SerializeField] private GameObject m_UnselectedState = null;

        private LearningsDisplayPanel m_Owner = null;

        private ScenarioSettings.LearningsExtension.LearningsEntry m_Data = null;

        public void Init(LearningsDisplayPanel owner, ScenarioSettings.LearningsExtension.LearningsEntry entry) 
        {
            m_Owner = owner;
            m_Data = entry;

            Array.ForEach(m_Texts, text => { text.text = LocalizationManager.Instance.GetLocalizedValue(m_Data.Title); });
        }

        public void OnDeselect(BaseEventData eventData)
        {
            m_SelectedState.SetActive(false);
            m_UnselectedState.SetActive(true);
        }

        public void OnSelect(BaseEventData eventData)
        {
            m_SelectedState.SetActive(true);
            m_UnselectedState.SetActive(false);
        }

        public void ShowContent() 
        {
            m_Owner.ShowContent(m_Data);   
        }
    }
}