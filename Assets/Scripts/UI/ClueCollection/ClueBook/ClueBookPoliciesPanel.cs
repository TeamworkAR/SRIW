using System.Collections.Generic;
using Data.ScenarioSettings;
using Managers;
using UnityEngine;

namespace UI.ClueCollection.ClueBook
{
    public class ClueBookPoliciesPanel : ClueBookUI.ClueBookPanel
    {
        [SerializeField] private List<PolicyDisplay> m_PolicyDisplays = new List<PolicyDisplay>(0);

        [SerializeField] private ClueBookTabButton m_TabButton = null;

        private ScenarioSettings.ClueCollectionExtension m_Extension = null;
        
        public override void Show()
        {
            base.Show();

            m_Extension = GameManager.Instance.ScenarioSettings
                .GetExtension<ScenarioSettings.ClueCollectionExtension>();

            if (m_PolicyDisplays.Count != m_Extension.PolicyWrappers.Count)
            {
                Debug.LogError(
                    $"{nameof(ClueBookThoughtDisplay)} authored are not enough to cover all {nameof(ScenarioSettings.ClueCollectionExtension.PoliciesWrapper)} in this scenario");
            }

            for (int i = 0; i < m_PolicyDisplays.Count; i++)
            {
                m_PolicyDisplays[i].FeedData(m_Extension.PolicyWrappers[i]);
            }

            m_TabButton.SetActiveViz();
        }

        public override void Hide()
        {
            base.Hide();
            
            m_PolicyDisplays.ForEach(t => t.Clear());

            m_TabButton.SetInactiveViz();
        }    
    }
}