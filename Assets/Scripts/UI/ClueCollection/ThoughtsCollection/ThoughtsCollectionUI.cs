using System.Collections;
using Data.ScenarioSettings;
using Managers;
using UnityEngine;
using UnityEngine.Events;

namespace UI.ClueCollection.ThoughtsCollection
{
    public class ThoughtsCollectionUI : BaseUICanvas
    {
        [SerializeField] private UnityEvent m_OnAllThoughtUnlocked = null;

        [SerializeField] private ThoughtsDisplayPanel m_ThoughtsDisplayPanel = null;
        
        private Coroutine m_Running = null;
        
        public override void Show()
        {
            base.Show();
            
            TryStartLifeCycle();
        }

        public override void Hide()
        {
            base.Hide();
            
            TryStopLifeCycle();
            
            m_ThoughtsDisplayPanel.Hide();
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
            var extension =
                GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.ClueCollectionExtension>();

            foreach (var extensionLeftGroupCharacter in extension.LeftGroupCharacters)
            {
                m_ThoughtsDisplayPanel.SelectCharacter(extensionLeftGroupCharacter);

                while (m_ThoughtsDisplayPanel.ThoughtsCollected == false && m_ThoughtsDisplayPanel.IsOnScreen())
                {
                    yield return null;
                }
                
                m_ThoughtsDisplayPanel.Hide();
            }
            
            foreach (var extensionLeftGroupCharacter in extension.RightGroupCharacters)
            {
                m_ThoughtsDisplayPanel.SelectCharacter(extensionLeftGroupCharacter);

                while (m_ThoughtsDisplayPanel.ThoughtsCollected == false && m_ThoughtsDisplayPanel.IsOnScreen())
                {
                    yield return null;
                }

                // Don't hide now thoughts display panel to allow fade out
                if (extension.RightGroupCharacters.IndexOf(extensionLeftGroupCharacter) !=
                    extension.RightGroupCharacters.Count - 1)
                {
                    m_ThoughtsDisplayPanel.Hide(); 
                }
            }
            
            m_OnAllThoughtUnlocked?.Invoke();
        }
    }
}