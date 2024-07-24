using Core;
using Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FlipCardSimple : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI m_Text = null;
    [SerializeField] private GameObject nextButton = null;
    [SerializeField] private GameObject extraInfo = null;
    [SerializeField] private Image m_Frame = null;
    [SerializeField] private bool isTutorialCard;
    [SerializeField] private List<GameObject> rotatingObjects = new List<GameObject>(0);
    [SerializeField] private bool isFlippableMultipleTimes = false; // New field to allow multiple flips
    [SerializeField] private Image dot1 = null; // New field for the first dot
    [SerializeField] private Image dot2 = null; // New field for the second dot
    [SerializeField] private Color activeDotColor = Color.black; // Color for the active dot
    [SerializeField] private Color inactiveDotColor = Color.gray; // Color for the inactive dot
    [SerializeField] private Image cardImage = null; // Image component to swap sprites
    [SerializeField] private Sprite frontSprite = null; // Sprite for the front side
    [SerializeField] private Sprite backSprite = null; // Sprite for the back side

    FlipCardSimpleData flipCardDecisionData;
    string frontText;
    string flippedText;

    private Coroutine running = null;
    private bool isRightEntry = false;
    private bool isFlipped = false;
    private bool isSelected = false;

    public bool IsAnimating => running != null;
    public bool IsRightEntry => isRightEntry;
    public bool IsSelected => isSelected;
    public bool IsCorrectlySelectedOrUnselected => ((isSelected && isRightEntry) || (!isSelected && !isRightEntry));
    public FlipCardSimpleData FlipCardData => flipCardDecisionData;

    public void FeedData(FlipCardSimpleData flipCardDecisionData)
    {
        isFlipped = false;
        isSelected = false;

        this.flipCardDecisionData = flipCardDecisionData;

        frontText = flipCardDecisionData.FrontText.GetLocalizedString();
        flippedText = flipCardDecisionData.FlippedText.GetLocalizedString();

        UpdateValues();
    }

    private void UpdateValues()
    {
        m_Text.text = isFlipped ? flippedText : frontText;

        if (extraInfo != null)
        {
            extraInfo.SetActive(!isFlipped);
        }

        // Update dot colors
        dot1.color = isFlipped ? inactiveDotColor : activeDotColor;
        dot2.color = isFlipped ? activeDotColor : inactiveDotColor;

        // Update card sprite
        cardImage.sprite = isFlipped ? backSprite : frontSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTutorialCard) return;

        Rotate();
    }

    public void OnSelected()
    {
        isSelected = !isSelected;
    }

    public void Rotate()
    {
        nextButton.SetActive(true);
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

    public void ResetViz()
    {
        isSelected = false;
        isFlipped = false;
        UpdateValues();

        // Reset dot colors
        dot1.color = activeDotColor;
        dot2.color = inactiveDotColor;

        // Reset card sprite
        cardImage.sprite = frontSprite;
    }
}
