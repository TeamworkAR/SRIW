using Core;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class ApplicationPauseManager : SingletonBehaviour<ApplicationPauseManager>
{
    //This should be broken into two classes, one manager and one back button
    [SerializeField]
    private GameObject m_pauseIcon;
    [SerializeField]
    private GameObject m_playIcon;
        
    [SerializeField] private TMP_Text m_Text = null;

    [SerializeField]
    private LocalizedString localizationPause;
    [SerializeField]
    private LocalizedString localizationPlay;
    public bool isPaused { get; private set; }

    private void Start()
    {
        LocalizationManager.Instance.OnLocalizationChange.AddListener(UpdateText);
        if (LocalizationManager.Instance.Initialized)
        {
            UpdateText();
        }
    }

    private void OnDestroy()
    {
        LocalizationManager.Instance.OnLocalizationChange.RemoveListener(UpdateText);
    }

    private void UpdateText()
    {
        // m_Text.text = LocalizationManager.Instance.GetLocalizedValue(isPaused?localizationPlay:localizationPause);
        // LayoutRebuilder.ForceRebuildLayoutImmediate(m_Text.transform.parent as RectTransform);
        var ab = this.GetComponentInChildren<AccessibleButton>();
        ab.m_Text = LocalizationManager.Instance.GetLocalizedValue(isPaused?localizationPlay:localizationPause);
        ab?.Select();
    }

    private void OnEnable()
    {
        UpdateText();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    
    public void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        m_playIcon.SetActive(true);
        m_pauseIcon.SetActive(false);
        Time.timeScale = 0f;
        UpdateText();
    }

    public void Resume()
    {
        isPaused = false;
        m_playIcon.SetActive(false);
        m_pauseIcon.SetActive(true);
        Time.timeScale = 1f;
        UpdateText();
    }
}
