using System.Collections;
using Core;
using Data.ScenarioSettings;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ClueCollection.ThoughtsCollection
{
    public class ThoughtDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject m_Locked = null;

        [SerializeField] private GameObject m_Unlocked = null;

        [SerializeField] private GameObject m_CheckMarkIcon = null;
        
        [SerializeField] private Image m_CircleFill = null;

        [SerializeField] private TextMeshProUGUI m_Text = null;
        
        private ScenarioSettings.ClueCollectionExtension.Thought m_Data = null;

        private static Coroutine m_Running = null;

        public bool IsThoughtPlaying() => m_Running != null;

        private void Start()
        {
            m_CircleFill.color = GameManager.Instance.DevSettings.UIGreen;
        }

        public void FeedData(ScenarioSettings.ClueCollectionExtension.Thought data)
        {
            m_Data = data;

            m_Text.text = data.GetText();
        }

        public void ReadThought()
        {
            m_Locked.SetActive(m_Data.IsUnlocked() == false);
            
            m_Unlocked.SetActive(m_Data.IsUnlocked());

            TryStartReadThought();
        }

        public void Interrupt()
        {
            TryStopReadThought();
            
            AudioManager.Instance.StopThoughts();

            m_CircleFill.fillAmount = 0f;
        }

        private void TryStartReadThought()
        {
            if (m_Data.IsUnlocked() || m_Running != null)
            {
                return;
            }

            m_Running = StartCoroutine(COR_ReadThought());
        }

        private void TryStopReadThought()
        {
            if (m_Running == null)
            {
                return;
            }
            
            StopCoroutine(m_Running);
            m_Running = null;
        }

        private IEnumerator COR_ReadThought()
        {
            m_Locked.SetActive(false);
            m_Unlocked.SetActive(true);
            
            yield return Helpers.UI.COR_Scale(m_Unlocked.transform, new Vector3(1f,0f,1f), Vector3.one, 0.5f);
            
            while (UAP_AccessibilityManager.IsSpeaking())
            {
                yield return new WaitForSeconds(0.00001f);
            }
            
            AudioManager.Instance.PlayThought(m_Data.GetAudioClip());

            yield return Helpers.UI.COR_FillImage(m_CircleFill, 0f, 1f, Helpers.Audio.GetAudioClipLenght(m_Data.GetAudioClip()));
            
            m_Text.gameObject.SetActive(false);
            m_CheckMarkIcon.gameObject.SetActive(true);
            
            yield return Helpers.UI.COR_Scale(m_CheckMarkIcon.transform, Vector3.zero, Vector3.one, 0.5f);

            m_Data.Unlock();
            
            TryStopReadThought();
        }

        private void OnDestroy()
        {
            TryStopReadThought();
        }
    }
}