using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FlipCardSelection : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI m_Text = null;
    [SerializeField] private Image m_Frame = null; // Frame to highlight the selected card
    [SerializeField] private Color selectedColor = Color.yellow; // Color for selected state
    [SerializeField] private Color defaultColor = Color.white; // Color for default state
    [SerializeField] private GameObject selectButton = null; // Button to show when a card is selected
    [SerializeField] private bool isRightEntry = false; // Whether this card is the correct answer
    [SerializeField] private string flippedText = ""; // Text to show when card is flipped

    private bool isSelected = false;
    private string frontText;
    private string backText;

    private UnityAction onClickCallback;

    public bool IsRightEntry => isRightEntry;
    public bool IsCorrectlySelectedOrUnselected => ((isSelected && isRightEntry) || (!isSelected && !isRightEntry));
    public string FlippedText => flippedText;

    public void SetTexts(string _frontText, string _backText)
    {
        frontText = _frontText;
        backText = _backText;
        UpdateValues();
    }

    private void UpdateValues()
    {
        m_Text.text = frontText;
        m_Frame.color = isSelected ? selectedColor : defaultColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        m_Frame.color = isSelected ? selectedColor : defaultColor;
        selectButton.SetActive(isSelected); // Activate or deactivate the select button based on the selection state
        onClickCallback?.Invoke();
    }

    public void SetOnClickCallback(UnityAction action)
    {
        onClickCallback = action;
    }

    public void ResetViz()
    {
        isSelected = false;
        UpdateValues();
        selectButton.SetActive(false); // Ensure the select button is hidden when resetting
    }

    public void FeedData(FlipCardDecisionData flipCardDecisionData)
    {
        isSelected = false;
        this.isRightEntry = flipCardDecisionData.IsRightEntry;
        frontText = flipCardDecisionData.FrontText.GetLocalizedString();
        flippedText = flipCardDecisionData.FlippedText.GetLocalizedString();
        UpdateValues();
    }

    public void ShowResult()
    {
        m_Frame.gameObject.SetActive(true); // Ensure frame is visible when showing result
        m_Text.text = flippedText;
    }
}
