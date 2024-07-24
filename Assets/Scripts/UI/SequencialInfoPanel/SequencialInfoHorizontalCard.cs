using Core;
using Managers;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class SequencialInfoHorizontalCard : MonoBehaviour
{
    [SerializeField] private GameObject m_Locked = null;
    [SerializeField] private GameObject m_Unlocked = null;
    [SerializeField] private Image circleFill = null;
    [SerializeField] private Image circleBase = null;
    [SerializeField] private TextMeshProUGUI persistentTitleText = null;
    [SerializeField] private SequencialAnswerButton awnserButton;

    [HideInInspector] public SequencialInfoPanelHorizontal cardController;
    [HideInInspector] public InfoCard cardData = null;

    [SerializeField] private bool skipLoadingUI = false; // Boolean to skip loading UI

    private static Coroutine running = null;

    public bool IsCardPlaying() => running != null;

    private void Start()
    {
        circleFill.color = GameManager.Instance.DevSettings.UIGreen;
    }

    public void FeedData(InfoCard card)
    {
        cardData = card;
        persistentTitleText.text = cardData.GetText();
    }

    public void ReadInfoCard()
    {
        if (skipLoadingUI)
        {
            ShowCardInstantly();
        }
        else
        {
            m_Locked.SetActive(true);
            m_Unlocked.SetActive(false);
            TryStartReadCard();
        }
    }

    public void Interrupt()
    {
        TryStopReadThought();
        AudioManager.Instance.StopThoughts();
        circleFill.fillAmount = 0f;
    }

    private void TryStartReadCard()
    {
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
        m_Locked.SetActive(false);
        m_Unlocked.SetActive(true);

        if (cardData.GetAudioClip() != null)
        {
            AudioManager.Instance.PlayThought(cardData.GetAudioClip());
            yield return Helpers.UI.COR_FillImage(circleFill, 0f, 1f, Helpers.Audio.GetAudioClipLenght(cardData.GetAudioClip()));
        }
        else
        {
            yield return Helpers.UI.COR_FillImage(circleFill, 0f, 1f, Helpers.UI.GetReadTime(cardData.GetText()) / 100);
        }

        TryStopReadThought();
    }

    private void ShowCardInstantly()
    {
        m_Locked.SetActive(false);
        m_Unlocked.SetActive(true);
        HideWheel();
        if (cardData.GetAudioClip() != null)
        {
            AudioManager.Instance.PlayThought(cardData.GetAudioClip());
        }
    }

    public void HideWheel()
    {
        circleBase.color = Color.black;
        circleFill.gameObject.SetActive(false);
    }

    public void SetCardController(SequencialInfoPanelHorizontal _cardController)
    {
        cardController = _cardController;
    }

    public void CorrectAwnserSelected()
    {
        cardController.CorrectAwsnerSelected();
    }

    public void SelectedAnwser()
    {
        circleBase.gameObject.SetActive(true);
        cardController.SelectedAwnser();
    }

    public void ActivateHiddenWheel()
    {
        circleBase.gameObject.SetActive(false);
    }

    public void ResetAwnser()
    {
        circleBase.gameObject.SetActive(true);
        awnserButton.ResetColor();
    }
}
