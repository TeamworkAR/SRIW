using Data.ScenarioSettings;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ClueCollection
{
    public class MainPanel : BaseUICanvas
    {
        [SerializeField] private Button m_ThoughtsButton = null;
        
        [SerializeField] private Button m_PoliciesButton = null;

        [SerializeField] private Button m_NextButton = null;

        [SerializeField] private GameObject m_TickCorrectPolicies = null;

        [SerializeField] private GameObject m_TickCorrectThoughts = null;
        
        public override void Show()
        {
            base.Show();

            var data = GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.ClueCollectionExtension>();

            m_ThoughtsButton.interactable =
                data.LeftGroupCharacters.TrueForAll(c => data.AreAllThoughtsUnlockedFor(c)) == false ||
                data.RightGroupCharacters.TrueForAll(c => data.AreAllThoughtsUnlockedFor(c)) == false;

            m_PoliciesButton.interactable = data.AreAllPoliciesUnlocked() == false;

            m_TickCorrectThoughts.SetActive(!m_ThoughtsButton.interactable);
            m_TickCorrectPolicies.SetActive(!m_PoliciesButton.interactable);

            m_NextButton.gameObject.SetActive(m_ThoughtsButton.interactable == false && m_PoliciesButton.interactable == false);
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}