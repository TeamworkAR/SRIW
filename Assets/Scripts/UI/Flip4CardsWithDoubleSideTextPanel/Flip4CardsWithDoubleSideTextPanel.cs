// Same usings as before
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Data.ScenarioSettings;
using UI;

public class Flip4CardsWithDoubleSideTextPanel : BaseUICanvas
{
    [Header("UI Components")]
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private List<DoubleSideFlipCard> flipCardsByOrder;

    private bool uiEnded = false;
    public bool IsDone() => uiEnded;

    // Track if cards have been flipped at least once
    [SerializeField] private bool[] cardFlippedOnce;

    public void ShowText(Flip4CardsWithDoubleSideTextDataWrapper wrapper)
    {
        Debug.Log("#QuestionWith3Options - Starting...");

        uiEnded = false;
        base.Show();

        // Hide the next button initially
        nextButton.SetActive(false);

        // Initialize the tracking array if it's null or mismatched size
        if (cardFlippedOnce == null || cardFlippedOnce.Length != flipCardsByOrder.Count)
        {
            cardFlippedOnce = new bool[flipCardsByOrder.Count];
        }

        // Set the message text from the data wrapper
        messageText.text = wrapper.data.MessageText.GetLocalizedString();

        // Initialize each card with its text and callback
        for (int i = 0; i < wrapper.data.CardTexts.Count; i++)
        {
            int localIndex = i;

            flipCardsByOrder[localIndex].SetTexts(
                wrapper.data.CardTexts[localIndex].Front.GetLocalizedString(),
                wrapper.data.CardTexts[localIndex].Back.GetLocalizedString()
            );

            flipCardsByOrder[localIndex].SetOnClickCallback(() => CheckIfCardFlipped(localIndex));
        }

        // Ensure the next button state is correctly restored
        RestoreNextButtonState();
    }

    // Check if a card has been flipped at least once
    private void CheckIfCardFlipped(int cardIndex)
    {
        if (!cardFlippedOnce[cardIndex])  // If this is the first time flipping the card
        {
            cardFlippedOnce[cardIndex] = true;  // Mark it as flipped
        }

        // Show the next button if all cards have been flipped at least once
        if (AllCardsFlippedOnce())
        {
            nextButton.SetActive(true);
        }
    }

    // Check if all cards have been flipped at least once
    private bool AllCardsFlippedOnce()
    {
        foreach (bool flipped in cardFlippedOnce)
        {
            if (!flipped) return false;
        }
        return true;
    }

    // Ensure the next button stays visible if all cards were flipped once
    private void RestoreNextButtonState()
    {
        if (AllCardsFlippedOnce())
        {
            nextButton.SetActive(true);
        }
        else
        {
            nextButton.SetActive(false);
        }
    }

    // Recheck card states when the panel is shown again
    public override void Show()
    {
        base.Show();
        RestoreNextButtonState();
        uiEnded = false;
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void EndPanel()
    {
        uiEnded = true;
        Hide();
    }

    private void Update()
    {
        // Ensure the next button stays active in case cards have been flipped
        RestoreNextButtonState();
    }
}
