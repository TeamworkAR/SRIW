using System.Collections;
using System.Collections.Generic;
using Animation;
using Data.CharacterData;
using Data.ScenarioSettings;
using Managers;
using TMPro;
using UI;
using UI.ClueCollection.ClueBook;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class SequencialInfoPanelHorizontal : BaseUICanvas
{
    [SerializeField] private GameObject infoCardContainer = null; // Container for info cards
    [SerializeField] private GameObject nextButton = null; // Button to proceed to the next section
    [SerializeField] private SequencialInfoHorizontalCard sequencialInfoCardTemplate = null; // Template for creating new info cards
    [SerializeField] private TextMeshProUGUI persistentTitleText = null; // Text for displaying the persistent title
    [SerializeField] private GameObject titleUIElement = null; // UI element for the title
    [SerializeField] private GameObject correctAwnserElement = null; // Element to display the correct answer
    [SerializeField] private TextMeshProUGUI correctAwnserText = null; // Text for the correct answer
    [SerializeField] private TextMeshProUGUI correctAwnserTitleText = null; // Title text for the correct answer

    private List<SequencialInfoHorizontalCard> spawnedSequencialInfoCards = new List<SequencialInfoHorizontalCard>(0); // List to store spawned info cards
    private Coroutine m_Running = null; // Reference to the running coroutine
    private ScenarioSettings.ClueCollectionExtension m_Extension = null; // Clue collection extension settings
    private bool b_NextPressed = false; // Flag to indicate if next button was pressed
    private List<SequencialInfoHorizontalCard> awnsersAvailable; // List to store available answers

    public bool SequencialInfoEnded => b_NextPressed && m_Running == null; // Property to check if sequence info has ended

    // Method to show standalone info cards
    public void ShowStandalone(SequencialInfoHorizontalCardDataWrapper wrapper)
    {
        base.Show();

        b_NextPressed = false;

        // Activate UI elements
        infoCardContainer.SetActive(true);
        correctAwnserElement.SetActive(false);
        nextButton.SetActive(false);
        awnsersAvailable = new List<SequencialInfoHorizontalCard>();

        // Create and initialize info cards
        foreach (var card in wrapper.SequencialInfoCardData.Cards)
        {
            SequencialInfoHorizontalCard infoCard = Instantiate(sequencialInfoCardTemplate, infoCardContainer.transform);
            infoCard.SetCardController(this);
            spawnedSequencialInfoCards.Add(infoCard);
            infoCard.FeedData(card);
            awnsersAvailable.Add(infoCard);
        }

        // Set persistent title text
        if (wrapper.SequencialInfoCardData.TitleString != null)
        {
            persistentTitleText.text = wrapper.SequencialInfoCardData.TitleString.GetLocalizedString();
            titleUIElement.SetActive(true);
        }
        else
        {
            titleUIElement.SetActive(false);
        }

        // Set correct answer text and title
        if (wrapper.SequencialInfoCardData.ConclusionString != null)
        {
            correctAwnserText.text = wrapper.SequencialInfoCardData.ConclusionString.GetLocalizedString();
        }

        if (wrapper.SequencialInfoCardData.ConclusionTitleString != null)
        {
            correctAwnserTitleText.text = wrapper.SequencialInfoCardData.ConclusionTitleString.GetLocalizedString();
        }

        nextButton.transform.SetAsLastSibling(); // Ensure next button is the last sibling in the hierarchy

        TryStartLifeCycle(); // Start the lifecycle coroutine
    }

    // Override method to show the UI canvas
    public override void Show()
    {
        base.Show();
    }

    // Override method to hide the UI canvas
    public override void Hide()
    {
        base.Hide();

        AudioManager.Instance.StopThoughts(); // Stop any playing audio

        // Deactivate and clear UI elements
        infoCardContainer.SetActive(false);
        nextButton.SetActive(false);
        CharacterShowcase.ClearByOwner(this);

        // Destroy spawned info cards
        foreach (var infoCard in spawnedSequencialInfoCards)
        {
            infoCard.StopAllCoroutines();
            Destroy(infoCard.gameObject);
        }
        spawnedSequencialInfoCards.Clear();

        TryStopLifeCycle(); // Stop the lifecycle coroutine
    }

    // Method to start the lifecycle coroutine
    private void TryStartLifeCycle()
    {
        if (m_Running != null)
        {
            return;
        }

        m_Running = StartCoroutine(COR_LifeCycle());
    }

    // Method to stop the lifecycle coroutine
    private void TryStopLifeCycle()
    {
        if (m_Running == null)
        {
            return;
        }

        StopCoroutine(m_Running);
        m_Running = null;
        Hide();
    }

    // Coroutine to manage the lifecycle of displaying post-its
    private IEnumerator COR_LifeCycle()
    {
        yield return null;

        RectTransform thoughtsContainerTransform = infoCardContainer.GetComponent<RectTransform>();

        // Iterate through the post-its on the current page
        for (int i = 0; i < spawnedSequencialInfoCards.Count; i++)
        {
            spawnedSequencialInfoCards[i].ReadInfoCard();

            // Wait until the current post-it is done displaying
            while (spawnedSequencialInfoCards[i].IsCardPlaying())
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(thoughtsContainerTransform);
                yield return null;
            }

            yield return new WaitForSeconds(0f);

            spawnedSequencialInfoCards[i].HideWheel();
        }

        //infoCardContainer.SetActive(false);

        //m_EndText.text = string.Format(m_EndLocalizedString.GetLocalizedString(), m_CharacterData.GetName());

        //m_HandPointing.SetActive(true);
    }

    // Method to handle the next button press
    public void PressNext()
    {
        b_NextPressed = true;
        TryStopLifeCycle();
    }

    // Method to handle the correct answer selected
    public void CorrectAwsnerSelected()
    {
        infoCardContainer.SetActive(false);
        correctAwnserElement.SetActive(true);
        nextButton.SetActive(true);
    }

    // Method to handle selected answer
    public void SelectedAwnser()
    {
        // Reset all answers
        foreach (SequencialInfoHorizontalCard hc in awnsersAvailable)
        {
            hc.ResetAwnser();
        }
    }
}
