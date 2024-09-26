using Data.ScenarioSettings;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Animation;

public class Flip4CardsWithDoubleSideTextPanel : BaseUICanvas
{
    [Header("UI components")]
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private List<DoubleSideFlipCard> flipCardsByOrder;

    private bool uiEnded = false;
    public bool IsDone() => uiEnded;

    [SerializeField] private int flipCounter = 0;
    [SerializeField] private bool[] cardFlippedOnce; // Array to track if each card has been flipped at least once

    /// <summary>
    /// Called by the Show action
    /// </summary>
    /// <param name="wrapper"></param>
    public void ShowText(Flip4CardsWithDoubleSideTextDataWrapper wrapper)
    {
        Debug.Log("#QuestionWith3Options - Starting...");

        uiEnded = false;

        base.Show();

        nextButton.SetActive(false); // Initially hide the next button

        flipCounter = 0;
        cardFlippedOnce = new bool[flipCardsByOrder.Count]; // Initialize the tracking array

        messageText.text = wrapper.data.MessageText.GetLocalizedString();

        for (int i = 0; i < wrapper.data.CardTexts.Count; i++)
        {
            int localIndex = i;

            // Set texts for the card
            flipCardsByOrder[localIndex].SetTexts(wrapper.data.CardTexts[localIndex].Front.GetLocalizedString(),
                wrapper.data.CardTexts[localIndex].Back.GetLocalizedString());

            // Use a callback every time the card is flipped so we can track if it's flipped
            flipCardsByOrder[localIndex].SetOnClickCallback(() => CheckIfCardFlipped(localIndex));
        }
    }

    // Check if each card has been flipped
    private void CheckIfCardFlipped(int cardIndex)
    {
        if (!cardFlippedOnce[cardIndex]) // If the card has not been flipped yet
        {
            cardFlippedOnce[cardIndex] = true; // Mark it as flipped
            flipCounter++;
        }

        // If all cards are flipped, enable the next button
        if (flipCounter >= flipCardsByOrder.Count)
        {
            nextButton.SetActive(true); // Show next button
        }
    }

    // Called when enabling the UI to check if cards are already flipped
    private void CheckAllCardsFlipped()
    {
        flipCounter = 0;
        for (int i = 0; i < flipCardsByOrder.Count; i++)
        {
            if (flipCardsByOrder[i].IsFlipped) // Using the IsFlipped property from DoubleSideFlipCard
            {
                flipCounter++;
                cardFlippedOnce[i] = true; // Mark this card as flipped
            }
        }

        // If all cards are flipped, enable the next button
        nextButton.SetActive(flipCounter >= flipCardsByOrder.Count);
    }

    // This will automatically be called when the UI becomes active
    private void Update()
    {
        CheckAllCardsFlipped(); // Check flipped cards when the UI is enabled
    }

    public override void Show()
    {
        base.Show();
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
}
