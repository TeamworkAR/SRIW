using Data.ScenarioSettings;
using System;
using TMPro;
using UI.ClueCollection.ClueBook;
using UnityEngine;

namespace Assets.Scripts.UI.ClueCollection.ClueBook
{
    public class ClueBookButton : MonoBehaviour
    {
        [SerializeField] private GameObject m_NotificationContainer = null;

        [SerializeField] private TextMeshProUGUI m_NotificationText = null;

        [SerializeField] private ClueBookNotificationAnimation[] m_ClueBookNotificationAnimation = null;

        private int m_NotificationCounter = 0;

        private void OnEnable()
        {
            ScenarioSettings.ClueCollectionExtension.UnlockableClue.onClueUnlocked += OnClueUnlocked;
            ScenarioSettings.ClueCollectionExtension.UnlockableClue.onClueLocked += OnClueLocked;

            ClueBookUI.OnClueBookShow += OnClueBookShow;

            m_NotificationContainer.SetActive(m_NotificationCounter > 0);
        }

        private void Start()
        {
            m_NotificationContainer.gameObject.SetActive(m_NotificationCounter > 0);
        }

        private void OnDisable()
        {
            ScenarioSettings.ClueCollectionExtension.UnlockableClue.onClueUnlocked -= OnClueUnlocked;
            ScenarioSettings.ClueCollectionExtension.UnlockableClue.onClueLocked -= OnClueLocked;

            ClueBookUI.OnClueBookShow -= OnClueBookShow;

            m_NotificationContainer.gameObject.SetActive(false);
        }

        private void OnClueUnlocked(ScenarioSettings.ClueCollectionExtension.UnlockableClue clue)
        {
            m_NotificationCounter++;

            m_NotificationContainer.gameObject.SetActive(true);

            // TODO: Consider number localization
            m_NotificationText.text = $"{m_NotificationCounter}";

            Array.ForEach(m_ClueBookNotificationAnimation, n => n.TryStartAnimation());
        }

        private void OnClueLocked(ScenarioSettings.ClueCollectionExtension.UnlockableClue clue)
        {
            m_NotificationCounter = Mathf.Max(0, m_NotificationCounter - 1);

            m_NotificationContainer.gameObject.SetActive(m_NotificationCounter > 0);

            // TODO: Consider number localization
            m_NotificationText.text = $"{m_NotificationCounter}";
        }

        private void OnClueBookShow()
        {
            m_NotificationCounter = 0;

            m_NotificationContainer.gameObject.SetActive(false);

            // TODO: Consider number localization
            m_NotificationText.text = $"{m_NotificationCounter}";
        }
    }
}