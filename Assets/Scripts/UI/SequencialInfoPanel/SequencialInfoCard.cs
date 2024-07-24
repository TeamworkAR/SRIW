using Core;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class SequencialInfoCard : MonoBehaviour
{
    [SerializeField] private GameObject m_Locked = null;
    [SerializeField] private GameObject m_Unlocked = null;
    [SerializeField] private Image circleFill = null;
    [SerializeField] private Image progressFill = null;
    [SerializeField] private TextMeshProUGUI persistentTitleText = null;
    [SerializeField] private float cardTime = 1.5f;
    [SerializeField] private bool hideRadialUIAfterLifecycle = true;  // New boolean field

    [HideInInspector] public InfoCard cardData = null;

    private static Coroutine running = null;

    public bool IsCardPlaying() => running != null;

    private void Start()
    {
        Debug.Log("Started first load");
        circleFill.color = GameManager.Instance.DevSettings.UIGreen;
    }

    public void FeedData(InfoCard card)
    {
        cardData = card;
        persistentTitleText.text = cardData.GetText();
    }

    public void ReadInfoCard()
    {
        m_Locked.SetActive(true);
        m_Unlocked.SetActive(false);
        TryStartReadCard();
    }

    public void Interrupt()
    {
        TryStopReadThought();
        AudioManager.Instance.StopThoughts();
        circleFill.fillAmount = 0f;
    }

    private void TryStartReadCard()
    {
        running = null;
        if (running != null)
        {
            return;
        }
        running = StartCoroutine(COR_ReadThought());
    }

    private void TryStopReadThought()
    {
        if (running == null)
        {
            return;
        }
        StopCoroutine(running);
        running = null;
    }

    private IEnumerator COR_ReadThought()
    {
        Debug.Log("Started read thought");
        m_Locked.SetActive(false);
        m_Unlocked.SetActive(true);
        if (cardData.GetAudioClip() != null)
        {
            AudioManager.Instance.PlayThought(cardData.GetAudioClip());
            yield return Helpers.UI.COR_FillImage(circleFill, 0f, 1f, Helpers.Audio.GetAudioClipLenght(cardData.GetAudioClip()));
        }
        else
        {
            yield return Helpers.UI.COR_FillImage(circleFill, 0f, 1f, Helpers.UI.GetReadTime(cardData.GetText()) / cardTime);
        }

        if (hideRadialUIAfterLifecycle)
        {
            HideProgressUI();
        }

        TryStopReadThought();
    }

    public void HideProgressUI()
    {
        foreach (var image in progressFill.GetComponentsInChildren<Image>(true))
        {
            image.enabled = false;
        }
    }
}

[Serializable]
public class InfoCard
{
    [SerializeField] private LocalizedString infoString = null;
    [SerializeField] private AudioClip infoVoiceOver = null;
    [SerializeField] public bool isCorrectAwnser = false;

    public string GetText() => infoString.GetLocalizedString();
    public AudioClip GetAudioClip() => infoVoiceOver;
}
