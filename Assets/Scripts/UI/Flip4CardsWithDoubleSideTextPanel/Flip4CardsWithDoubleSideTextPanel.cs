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

        nextButton.SetActive(false);

        flipCounter = 0;
        cardFlippedOnce = new bool[flipCardsByOrder.Count]; // Initialize the tracking array

        messageText.text = wrapper.data.MessageText.GetLocalizedString();

        for (int i = 0; i < wrapper.data.CardTexts.Count; i++)
        {
            // Create a local copy of the index to avoid closure issues
            int localIndex = i;

            // Set texts for the card
            flipCardsByOrder[localIndex].SetTexts(wrapper.data.CardTexts[localIndex].Front.GetLocalizedString(),
                wrapper.data.CardTexts[localIndex].Back.GetLocalizedString());

            // Use a callback every time the button is pressed so we can know when all are flipped
            flipCardsByOrder[localIndex].SetOnClickCallback(() => CheckIfCardFlipped(localIndex));
        }
    }

    private void CheckIfCardFlipped(int cardIndex)
    {
        // Check if the card was flipped for the first time
        if (!cardFlippedOnce[cardIndex])
        {
            cardFlippedOnce[cardIndex] = true;
            flipCounter++;
        }

        // Check if all cards are flipped at least once
        if (flipCounter >= flipCardsByOrder.Count)
        {
            nextButton.SetActive(true);
        }
    }

    // New method to check if all cards are flipped when enabling the UI
    private void CheckAllCardsFlipped()
    {
        flipCounter = 0;
        for (int i = 0; i < flipCardsByOrder.Count; i++)
        {
            // Assuming the DoubleSideFlipCard class has a property IsFlipped to check if the card is flipped
            if (flipCardsByOrder[i].IsFlipped)
            {
                cardFlippedOnce[i] = true;
                flipCounter++;
            }
        }

        // If all cards were flipped, enable the next button
        if (flipCounter >= flipCardsByOrder.Count)
        {
            nextButton.SetActive(true);
        }
    }

    public override void Show()
    {
        base.Show();

        uiEnded = false;

        // Check the state of all cards when showing the panel
        CheckAllCardsFlipped();
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
