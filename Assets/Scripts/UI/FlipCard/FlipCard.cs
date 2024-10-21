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
    [Header("UI Elements")]
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
    [SerializeField] private bool isSelectAlwaysActive = false;
    [SerializeField] private Image dot1 = null;
    [SerializeField] private Image dot2 = null;
    [SerializeField] private Color activeDotColor = Color.black;
    [SerializeField] private Color inactiveDotColor = Color.gray;
    [SerializeField] private Image cardImage = null;
    [SerializeField] private Sprite frontSprite = null;
    [SerializeField] private Sprite backSprite = null;
    [SerializeField] private bool isWrong = false;

    [Header("Select Button Color Settings")]
    [SerializeField] private Color selectButtonActiveColor = Color.green;  // Active color for selection
    [SerializeField] private Color selectButtonDefaultColor = Color.white; // Default color when not selected

    private Image selectButtonImage;  // Reference to the Image component on the select button

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
    public bool IsCorrectlySelectedOrUnselected => (isSelected && isRightEntry) || (!isSelected && !isRightEntry);
    public FlipCardDecisionData FlipCardData => flipCardDecisionData;
    public string FlippedText => flippedText;

    public UnityAction OnCardFlipped;
    public UnityAction OnCardSelected;

    private void Awake()
    {
        // Get the Image component from the select button
        if (selectButton != null)
        {
            selectButtonImage = selectButton.GetComponent<Image>();
        }
    }

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

        if (isSelectAlwaysActive)
        {
            selectButton.SetActive(true);
        }
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
        if (isTutorialCard || isSelectAlwaysActive) return;

        Rotate();
    }

    public void ShowSelectButton()
    {
        selectButton.SetActive(true);
    }

    public void OnSelected()
    {
        isSelected = !isSelected;  // Toggle selection state

        // Change the frame's visibility and color
        m_Frame.gameObject.SetActive(isSelected);
        m_Frame.color = isSelected ? selectedColor : defaultColor;

        // Change the select button's color
        if (selectButtonImage != null)
        {
            selectButtonImage.color = isSelected ? selectButtonActiveColor : selectButtonDefaultColor;
        }

        // Trigger the OnCardSelected event
        OnCardSelected?.Invoke();
    }

    public void Rotate()
    {
        if (isSelectAlwaysActive)
        {
            ShowSelectButton();
            return;
        }

        selectButton.SetActive(true);
        if (isFlippableMultipleTimes || !isFlipped)
        {
            TryStartLifecycle();
        }
    }

    private IEnumerator COR_NextValueLifeCycle()
    {
        const float rotateDuration = 0.2f;
        yield return Helpers.UI.COR_Rotate(rotatingObjects, new Vector3(0f, 90f, 0f), rotateDuration);
        isFlipped = !isFlipped;
        UpdateValues();
        yield return Helpers.UI.COR_Rotate(rotatingObjects, Vector3.zero, rotateDuration);
        TryStopLifecycle();

        OnCardFlipped?.Invoke();
    }

    private void TryStartLifecycle()
    {
        if (running != null) return;
        running = StartCoroutine(COR_NextValueLifeCycle());
    }

    private void TryStopLifecycle()
    {
        if (running == null) return;
        StopCoroutine(running);
        running = null;
    }

    public void ShowResult()
    {
        m_Frame.gameObject.SetActive(true);
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
        m_Frame.gameObject.SetActive(false);
        isSelected = false;
        isFlipped = false;
        m_TickCorrect.SetActive(false);
        m_CrossWrong.SetActive(false);

        selectButton.SetActive(isSelectAlwaysActive);

        dot1.color = activeDotColor;
        dot2.color = inactiveDotColor;
        cardImage.sprite = frontSprite;

        if (selectButtonImage != null)
        {
            selectButtonImage.color = selectButtonDefaultColor;
        }
    }
}
