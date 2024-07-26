using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

public class SequencialInfoPostIt : BaseUICanvas
{
    //[SerializeField] private RawImage characterDisplay = null; // Reference to the character display image
    [SerializeField] private GameObject infoCardContainer = null; // Container for info cards
    [SerializeField] private GameObject nextButton = null; // Button to go to the next set of post-its
    [SerializeField] private GameObject previousButton = null; // Button for loading previous set of post-its
    [SerializeField] private GameObject loadNextButton = null; // Button for loading next set of post-its
    [SerializeField] private SequencialInfoCard sequencialInfoCardTemplate = null; // Template for creating new info cards
    [SerializeField] private TextMeshProUGUI persistentTitleText = null; // Text for displaying the persistent title
    [SerializeField] private GameObject titleUIElement = null; // UI element for the title
    [SerializeField] private bool hideCardsAfterLifecycle = true; // Option to hide cards after lifecycle
    [SerializeField] private bool hideRadialUIAfterLifecycle = true; // Option to hide radial UI after lifecycle
    [SerializeField] private int postItsPerPage = 2; // Number of post-its to display per page

    private List<SequencialInfoCard> spawnedSequencialInfoCards = new List<SequencialInfoCard>(0); // List to store spawned info cards
    //private CharacterData m_CharacterData = null; // Current character data
    private Coroutine m_Running = null; // Reference to the running coroutine
    private ScenarioSettings.ClueCollectionExtension m_Extension = null; // Clue collection extension settings
    private bool b_NextPressed = false; // Flag to indicate if next button was pressed
    [SerializeField] private bool pageLoaded = false; // Flag to indicate if next button was pressed
    [SerializeField] private bool postItsLoaded = false; // Flag to indicate if next button was pressed
    private int currentPage = 0; // Current page index
    private int currentPostItIndex = 0; // Current post-it index
    private bool pauseCoroutine = false; // Flag to pause the coroutine
    private bool continueCoroutine = false; // Flag to continue the coroutine

    public bool SequencialInfoEnded => b_NextPressed && m_Running == null; // Property to check if sequence info has ended

    // Method to show standalone info cards
    public void ShowStandalone(SequencialInfoCardDataWrapper wrapper)
    {
        base.Show();
        postItsLoaded = false;
        //m_CharacterData = wrapper.SequencialInfoCardData.Owner;
        b_NextPressed = false;

        // Activate UI elements
        infoCardContainer.SetActive(true);
        nextButton.SetActive(false);
        previousButton.SetActive(false);
        loadNextButton.SetActive(false);

        // Set character display
        //CharacterShowcase characterShowcase = m_CharacterData.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);
        //characterShowcase.GetComponent<ThoughtCollectionAnimations>().HandleThoughtCollection();
        //characterDisplay.texture = characterShowcase.ImageTexture;

        // Create and initialize info cards
        foreach (var card in wrapper.SequencialInfoCardData.Cards)
        {
            SequencialInfoCard infoCard = Instantiate(sequencialInfoCardTemplate, infoCardContainer.transform);
            spawnedSequencialInfoCards.Add(infoCard);
            infoCard.FeedData(card);
            infoCard.gameObject.SetActive(false);
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

        // Initialize pagination
        currentPage = 0;
        currentPostItIndex = 0;
        UpdatePostItsVisibility();
        TryStartLifeCycle();
    }

    // Method to update visibility of post-its based on the current page
    private void UpdatePostItsVisibility()
    {
        for (int i = 0; i < spawnedSequencialInfoCards.Count; i++)
        {
            spawnedSequencialInfoCards[i].gameObject.SetActive(i >= currentPage * postItsPerPage && i < (currentPage + 1) * postItsPerPage);
        }

        // Hide navigation buttons initially
        if (postItsLoaded)
        {
            previousButton.SetActive(currentPage > 0);
        }

        // Shows next page button when post its load unless it's the last page
        if (pageLoaded)
        {
            loadNextButton.SetActive(!IsLastPage());
        }
        
    }

    // Method to load the next page of post-its
    public void LoadNextPage()
    {
        if ((currentPage + 1) * postItsPerPage < spawnedSequencialInfoCards.Count)
        {
            loadNextButton.SetActive(false);
            currentPage++;
            UpdatePostItsVisibility();
            ResumeLifeCycle();
        }
    }

    // Method to load the previous page of post-its
    public void LoadPreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePostItsVisibility();
            if (postItsLoaded)
            {
                previousButton.SetActive(currentPage > 0);
            }
            loadNextButton.SetActive(true);
            pageLoaded = true;
        }
    }

    // Method to show the UI canvas
    public override void Show()
    {
        base.Show();
    }

    // Method to hide the UI canvas
    public override void Hide()
    {
        base.Hide();
        AudioManager.Instance.StopThoughts();

        // Deactivate and clear UI elements
        infoCardContainer.SetActive(false);
        nextButton.SetActive(false);
        previousButton.SetActive(false);
        loadNextButton.SetActive(false);
        CharacterShowcase.ClearByOwner(this);

        // Destroy spawned info cards
        foreach (var infoCard in spawnedSequencialInfoCards)
        {
            infoCard.StopAllCoroutines();
            Destroy(infoCard.gameObject);
        }
        spawnedSequencialInfoCards.Clear();

        TryStopLifeCycle();
    }

    // Method to start the lifecycle coroutine
    private void TryStartLifeCycle()
    {
        if (m_Running != null)
        {
            StopCoroutine(m_Running); // Ensure the previous coroutine is stopped
        }
        pauseCoroutine = false;
        continueCoroutine = true; // Ensure the coroutine will continue
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

    // Method to check if the current page is the last page
    private bool IsLastPage()
    {
        return (currentPage + 1) * postItsPerPage >= spawnedSequencialInfoCards.Count;
    }

    // Coroutine to manage the lifecycle of displaying post-its
    private IEnumerator COR_LifeCycle()
    {
        yield return null;

        RectTransform thoughtsContainerTransform = infoCardContainer.GetComponent<RectTransform>();

        // Iterate through the post-its on the current page
        for (int i = currentPostItIndex; i < spawnedSequencialInfoCards.Count; i++)
        {
            if (pauseCoroutine)
            {
                continueCoroutine = false;
                if (postItsLoaded)
                {
                    previousButton.SetActive(currentPage > 0);
                }
                loadNextButton.SetActive(true);
                currentPostItIndex = i; // Save the current index to continue from here later
                yield break; // Stop coroutine execution here
            }
            pageLoaded = false;
            spawnedSequencialInfoCards[i].gameObject.SetActive(true);
            spawnedSequencialInfoCards[i].ReadInfoCard();

            // Wait until the current post-it is done displaying
            while (spawnedSequencialInfoCards[i].IsCardPlaying())
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(thoughtsContainerTransform);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            if (hideCardsAfterLifecycle && i != spawnedSequencialInfoCards.Count - 1)
            {
                spawnedSequencialInfoCards[i].gameObject.SetActive(false);
            }

            // Pause the coroutine if we've reached the end of the current page
            if ((i + 1) % postItsPerPage == 0)
            {
                pauseCoroutine = true;
            }
        }

        // Optionally hide the radial UI for all post-its
        if (hideRadialUIAfterLifecycle)
        {
            foreach (var infoCard in spawnedSequencialInfoCards)
            {
                infoCard.HideProgressUI();
            }
        }
        pageLoaded = true;
        // Show navigation buttons based on the current page
        nextButton.SetActive(currentPage == (spawnedSequencialInfoCards.Count - 1) / postItsPerPage); // Show next button only on the last page
        postItsLoaded = currentPage == (spawnedSequencialInfoCards.Count - 1) / postItsPerPage; // Lets system know all post its have loaded and we can use the previous button
        
        if (postItsLoaded)
        {
            previousButton.SetActive(currentPage > 0); // Ensure previous button does not appear on the first page
        }

    }

    // Method to resume the lifecycle coroutine
    private void ResumeLifeCycle()
    {
        if (pauseCoroutine)
        {
            pauseCoroutine = false;
            if (!continueCoroutine)
            {
                TryStartLifeCycle(); // Restart the coroutine to continue from where it left off
            }
        }
    }

    // Method to select a character and show their info cards
    public void SelectCharacter(CharacterData data)
    {
        if (GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.ClueCollectionExtension>().GetThoughtsFor(data, true).Count(t => t.IsUnlocked() == false) == 0)
        {
            return;
        }

        //m_CharacterData = data;
        Show();
    }

    // Method to handle the next button press
    public void PressNext()
    {
        b_NextPressed = true;
        TryStopLifeCycle();
    }
}