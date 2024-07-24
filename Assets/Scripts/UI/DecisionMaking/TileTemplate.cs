using Data.ScenarioSettings;
using Managers;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DecisionMaking
{
    public class TileTemplate : MonoBehaviour
    {
        [SerializeField] private Canvas m_Canvas = null;

        [SerializeField] private TextMeshProUGUI[] m_Text = null;

        [SerializeField] private Image[] m_Image = null;

        [SerializeField] private Image m_Frame = null;

        [SerializeField] private GameObject m_TickCorrect = null;

        [SerializeField] private GameObject m_CrossWrong = null;

        [SerializeField] private GameObject m_ActiveState = null;

        [SerializeField] private GameObject m_InactiveState = null;
        
        [SerializeField] private AccessibleLabel m_AccessibleLabel = null;

        public AccessibleLabel AccessibleLabel => m_AccessibleLabel;


        private ScenarioSettings.DecisionMakingExtension.Entry m_Entry = null;

        public void SetLayer(int i)
        {
            m_Canvas.sortingOrder = i;
        }

        public void FeedData(ScenarioSettings.DecisionMakingExtension.Entry entry)
        {
            m_Entry = entry;

            Array.ForEach(m_Text, text => { text.text = m_Entry.GetText(); });

            //if (entry.Sprite != null)
            //{
            //    Array.ForEach(m_Image, image => { image.sprite = m_Entry.Sprite; });
            //}
        }

        public void ShowResult()
        {
            m_Frame.color = m_Entry.IsRightEntry
                ? GameManager.Instance.DevSettings.CorrectAnswerColor
                : GameManager.Instance.DevSettings.WrongAnswerColor;

            m_TickCorrect.SetActive(m_Entry.IsRightEntry);

            m_CrossWrong.SetActive(m_Entry.IsRightEntry == false);
        }

        public void ResetViz()
        {
            m_Frame.color = GameManager.Instance.DevSettings.FoundationalGold;
            m_TickCorrect.SetActive(false);
            m_CrossWrong.SetActive(false);
            m_AccessibleLabel.enabled = false;
        }

        public void SetActiveState()
        {
            m_ActiveState.SetActive(true);
            m_InactiveState.SetActive(false);
            m_AccessibleLabel.enabled = true;
        }

        public void SetInactiveState()
        {
            m_ActiveState.SetActive(false);
            m_InactiveState.SetActive(true);
            m_AccessibleLabel.enabled = false;
        }
    }
}