using System.Collections.Generic;
using UnityEngine;

namespace UI.Learnings
{
    // TODO: derive from panel container
    public class LearningsUI : BaseUICanvasGroup
    {
        [SerializeField] private List<LearningsUIPanel> m_Panels = new List<LearningsUIPanel>(0);

        private LearningsUIPanel m_CurrentPanel = null;

        private bool b_LearningsEnded = false;

        public override bool IsDone() => base.IsDone() && b_LearningsEnded;

        protected override void OnShowStart()
        {
            base.OnShowStart();
            
            b_LearningsEnded = false;
            
            m_CurrentPanel = m_Panels[0];
            
            m_CurrentPanel.Show();
        }

        protected override void OnHideCompleted()
        {
            base.OnHideCompleted();
            
            m_CurrentPanel.Hide();
        }

        public void NextPanel()
        {
            int currentIndex = m_Panels.IndexOf(m_CurrentPanel);
            if (currentIndex < m_Panels.Count - 1)
            {
                m_CurrentPanel.Hide();
                
                m_CurrentPanel = m_Panels[currentIndex + 1];
                
                m_CurrentPanel.Show();
            }
        }

        public void PreviousPanel()
        {
            int currentIndex = m_Panels.IndexOf(m_CurrentPanel);
            if (currentIndex > 0)
            {
                m_CurrentPanel.Hide();
                
                m_CurrentPanel = m_Panels[currentIndex - 1];
                
                m_CurrentPanel.Show();
            }
        }

        public void EndLearnings()
        {
            b_LearningsEnded = true;
            
            Hide();
        }
    }

    public class LearningsUIPanel : BaseUICanvas
    {
        
    }
}