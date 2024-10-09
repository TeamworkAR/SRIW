using Core;
using Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FlipCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI m_Text = null;
    [SerializeField] private GameObject selectButton = null;
    [SerializeField] private GameObject m_TickCorrect = null;
    [SerializeField] private GameObject m_CrossWrong = null;
    [SerializeField] private Image m_Frame = null;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private bool isTutorialCard;
    [SerializeField] private List<GameObject> rotatingObjects = new List<GameObject>(0);
    [SerializeField] private bool isFlippableMultipleTimes = false;
    [SerializeField] private Image dot1 = null;
    [SerializeField] private Image dot2 = null;
    [SerializeField] private Color activeDotColor = Color.black;
    [SerializeField] private Color inactiveDotColor = Color.gray;
    [SerializeField] private Image cardImage = null;
    [SerializeField] private Sprite frontSprite = null;
    [SerializeField] private Sprite backSprite = null;
    [SerializeField] private bool isWrong = false;

    FlipCardDecisionData flipCardDecisionData;
    string frontText;
    string flippedText;
    string incorrectText;

    private Coroutine running = null;
    private bool isRightEntry = false;
    private bool isFlipped = false;
    private bool isSelected = false;

    public bool IsAnimating => running != null;
    public bool IsRightEntry => isRightEntry;
    public bool IsSelected => isSelected;
    public bool IsCorrectlySelectedOrUnselected => ((isSelected && isRightEntry) || (!isSelected && !isRightEntry));
    public FlipCardDecisionData FlipCardData => flipCardDecisionData;
    public string FlippedText => flippedText;

    // Define an event that is triggered when the card is flipped
    public UnityAction OnCardFlipped;

    public void FeedData(FlipCardDecisionData flipCardDecisionData)
    {
        isFlipped = false;
        isSelected = false;
        this.flipCardDecisionData = flipCardDecisionData;
        isRightEntry = flipCardDecisionData.IsRightEntry;
        frontText = flipCardDecisionData.FrontText.GetLocalizedString();
        flippedText = flipCardDecisionData.FlippedText.GetLocalizedString();
        if (isWrong)
        {
            incorrectText = flipCardDecisionData.IncorrectAnswerResponse.GetLocalizedString();
        }
        UpdateValues();
    }

    private void UpdateValues()
    {
        m_Text.text = isFlipped ? flippedText : frontText;

        dot1.color = isFlipped ? inactiveDotColor : activeDotColor;
        dot2.color = isFlipped ? activeDotColor : inactiveDotColor;

        cardImage.sprite = isFlipped ? backSprite : frontSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTutorialCard) return;

        ShowSelectButton();
    }

    public void ShowSelectButton()
    {
        // Ensure the select button is visible
        selectButton.SetActive(true);
    }

    public void OnSelected()
    {
        isSelected = !isSelected;
        m_Frame.color = isSelected ? selectedColor : defaultColor;
        m_Frame.gameObject.SetActive(isSelected); // Activate or deactivate the frame based on the selection state
    }

    public void Rotate()
    {
        selectButton.SetActive(true);
        if (isFlippableMultipleTimes || !isFlipped)
        {
            TryStartLifecycle();
        }
    }

    private IEnumerator COR_NextValueLifeCycle()
    {
        yield return Helpers.UI.COR_Rotate(rotatingObjects, new Vector3(0f, 90f, 0f), 0.2f);
        isFlipped = !isFlipped;
        UpdateValues();
        yield return Helpers.UI.COR_Rotate(rotatingObjects, Vector3.zero, 0.2f);
        TryStopLifecycle();

        // Trigger the OnCardFlipped event when the card is flipped
        OnCardFlipped?.Invoke();
    }

    private void TryStartLifecycle()
    {
        if (running != null)
        {
            return;
        }
        running = StartCoroutine(COR_NextValueLifeCycle());
    }

    private void TryStopLifecycle()
    {
        if (running == null)
        {
            return;
        }
        StopCoroutine(running);
        running = null;
    }

    public void ShowResult()
    {
        m_Frame.gameObject.SetActive(true); // Ensure frame is visible when showing result
        if (isRightEntry)
        {
            m_Frame.color = GameManager.Instance.DevSettings.CorrectAnswerColor;
            m_TickCorrect.SetActive(true);
            selectButton.SetActive(false);
            m_CrossWrong.SetActive(false);
        }
        else
        {
            m_Frame.color = GameManager.Instance.DevSettings.WrongAnswerColor;
            if (isWrong)
            {
                m_Text.text = incorrectText;
            }
            selectButton.SetActive(false);
            m_CrossWrong.SetActive(true);
            m_TickCorrect.SetActive(false);
        }
    }

    public void ResetViz()
    {
        m_Frame.color = defaultColor;
        m_Frame.gameObject.SetActive(false); // Deactivate the frame initially
        isSelected = false;
        isFlipped = false;
        m_TickCorrect.SetActive(false);
        m_CrossWrong.SetActive(false);
        selectButton.SetActive(false);

        dot1.color = activeDotColor;
        dot2.color = inactiveDotColor;

        cardImage.sprite = frontSprite;
    }
}
