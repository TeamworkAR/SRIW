using System.Collections;
using System.Collections.Generic;
using Animation;
using Data.CharacterData;
using Data.ScenarioSettings;
using Managers;
using TMPro;
using UI;
using UI.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class SequencialInfoPanel : BaseUICanvas
{
    [SerializeField] private GameObject infoCardContainer = null;
    [SerializeField] private GameObject nextButton = null; // Button to proceed to the next module or section
    [SerializeField] private GameObject loadNextButton = null; // Button for loading the next set of post-its
    [SerializeField] private GameObject loadPreviousButton = null; // Button for loading the previous set of post-its
    [SerializeField] private SequencialInfoCard sequencialInfoCardTemplate = null;
    [SerializeField] private TextMeshProUGUI persistentTitleText = null;
    [SerializeField] private GameObject titleUIElement = null;
    [SerializeField] private CarouselCounterContainer carouselCounterContainer = null; // Carousel counter container

    [SerializeField] private int postItsPerPage = 3; // Number of post-its to display per page

    private List<SequencialInfoCard> spawnedSequencialInfoCards = new List<SequencialInfoCard>(0);
    private Coroutine m_Running = null;
    private ScenarioSettings.ClueCollectionExtension m_Extension = null;
    private bool b_NextPressed = false;

    private int currentPage = 0; // Current page index
    private int currentPostItIndex = 0; // Current post-it index
    private bool pauseCoroutine = false; // Flag to pause the coroutine
    private bool continueCoroutine = false; // Flag to continue the coroutine
    private bool pageLoaded = false; // Flag to indicate if the page is loaded
    [SerializeField] private int panelsToLoad = 0; // Number of panels to load on the current page
    [SerializeField] private int panelsLoaded = 0; // Number of panels that have finished their lifecycle
    private HashSet<int> loadedPages = new HashSet<int>(); // Track fully loaded pages

    public bool SequencialInfoEnded => b_NextPressed && m_Running == null;

    public void ShowStandalone(SequencialInfoCardDataWrapper wrapper)
    {
        base.Show();

        b_NextPressed = false;

        infoCardContainer.SetActive(true);
        nextButton.SetActive(false); // Keep the old functionality for nextButton
        loadNextButton.SetActive(true); // Ensure loadNextButton is always active
        loadNextButton.GetComponent<Button>().interactable = false; // Set to non-interactable initially
        loadPreviousButton.SetActive(true); // Ensure loadPreviousButton is always active
        loadPreviousButton.GetComponent<Button>().interactable = false; // Set to non-interactable initially
        carouselCounterContainer.Dispose(); // Clear any existing dots

        foreach (var card in wrapper.SequencialInfoCardData.Cards)
        {
            SequencialInfoCard infoCard = Instantiate(sequencialInfoCardTemplate, infoCardContainer.transform);
            spawnedSequencialInfoCards.Add(infoCard);
            infoCard.FeedData(card);
            infoCard.gameObject.SetActive(false);
        }

        if (wrapper.SequencialInfoCardData.TitleString != null)
        {
            persistentTitleText.text = wrapper.SequencialInfoCardData.TitleString.GetLocalizedString();
            titleUIElement.SetActive(true);
        }
        else
        {
            titleUIElement.SetActive(false);
        }

        nextButton.transform.SetAsLastSibling();

        if (spawnedSequencialInfoCards.Count > 1)
        {
            carouselCounterContainer.Init(Mathf.CeilToInt((float)spawnedSequencialInfoCards.Count / postItsPerPage));
        }

        // Initialize pagination
        currentPage = 0;
        currentPostItIndex = 0;
        loadedPages.Clear(); // Clear loaded pages
        UpdatePostItsVisibility();
        EnsureCarouselCounterIsBottomChild(); // Ensure carousel counter is at the bottom
        TryStartLifeCycle();
    }

    private void UpdatePostItsVisibility()
    {
        for (int i = 0; i < spawnedSequencialInfoCards.Count; i++)
        {
            spawnedSequencialInfoCards[i].gameObject.SetActive(i >= currentPage * postItsPerPage && i < (currentPage + 1) * postItsPerPage);
        }

        loadNextButton.GetComponent<Button>().interactable = false; // Set to non-interactable initially
        loadPreviousButton.GetComponent<Button>().interactable = false; // Set to non-interactable initially
        nextButton.SetActive(false); // Keep the old functionality for nextButton
        carouselCounterContainer.Select(currentPage); // Update the selected dot

        EnsureCarouselCounterIsBottomChild(); // Ensure carousel counter is at the bottom
    }

    public void LoadNextPage()
    {
        if ((currentPage + 1) * postItsPerPage < spawnedSequencialInfoCards.Count)
        {
            currentPage++;
            currentPostItIndex = currentPage * postItsPerPage;
            UpdatePostItsVisibility();
            if (loadedPages.Contains(currentPage))
            {
                ShowLoadedPage(); // Show loaded page instantly
            }
            else
            {
                TryStartLifeCycle(); // Start the lifecycle coroutine to load the next set of panels
            }
        }
    }

    public void LoadPreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            currentPostItIndex = currentPage * postItsPerPage;
            UpdatePostItsVisibility();
            if (loadedPages.Contains(currentPage))
            {
                ShowLoadedPage(); // Show loaded page instantly
            }
            else
            {
                TryStartLifeCycle(); // Start the lifecycle coroutine to load the previous set of panels
            }
        }
    }

    private void ShowLoadedPage()
    {
        int endPostItIndex = Mathf.Min(currentPostItIndex + postItsPerPage, spawnedSequencialInfoCards.Count);
        for (int i = currentPostItIndex; i < endPostItIndex; i++)
        {
            spawnedSequencialInfoCards[i].gameObject.SetActive(true);
        }

        loadNextButton.GetComponent<Button>().interactable = (currentPage + 1) * postItsPerPage < spawnedSequencialInfoCards.Count; // Make sure the button is interactable
        nextButton.SetActive((currentPage + 1) * postItsPerPage >= spawnedSequencialInfoCards.Count); // Show nextButton if this is the last page
        nextButton.GetComponent<Button>().interactable = (currentPage + 1) * postItsPerPage >= spawnedSequencialInfoCards.Count; // Make sure the button is interactable
        loadPreviousButton.GetComponent<Button>().interactable = currentPage > 0; // Make sure the button is interactable

        if (currentPage == (spawnedSequencialInfoCards.Count - 1) / postItsPerPage)
        {
            Debug.Log("Last card loaded. Next button is active and interactable.");
        }
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
        AudioManager.Instance.StopThoughts();

        // Deactivate and clear UI elements
        infoCardContainer.SetActive(false);
        nextButton.SetActive(false); // Keep the old functionality for nextButton
        loadNextButton.SetActive(false);
        loadPreviousButton.SetActive(false);
        CharacterShowcase.ClearByOwner(this);
        carouselCounterContainer.Dispose();

        // Destroy spawned info cards
        foreach (var infoCard in spawnedSequencialInfoCards)
        {
            infoCard.StopAllCoroutines();
            Destroy(infoCard.gameObject);
        }
        spawnedSequencialInfoCards.Clear();

        TryStopLifeCycle();
    }

    private void TryStartLifeCycle()
    {
        if (m_Running != null)
        {
            StopCoroutine(m_Running); // Ensure the previous coroutine is stopped
        }
        pauseCoroutine = false;
        continueCoroutine = true; // Ensure the coroutine will continue
        panelsToLoad = Mathf.Min(postItsPerPage, spawnedSequencialInfoCards.Count - currentPostItIndex);
        panelsLoaded = 0;
        m_Running = StartCoroutine(COR_LifeCycle());
    }

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

    private IEnumerator COR_LifeCycle()
    {
        yield return null;

        RectTransform thoughtsContainerTransform = infoCardContainer.GetComponent<RectTransform>();

        loadNextButton.GetComponent<Button>().interactable = false; // Set to non-interactable initially
        loadPreviousButton.GetComponent<Button>().interactable = false; // Set to non-interactable initially
        nextButton.SetActive(false); // Keep the old functionality for nextButton

        int endPostItIndex = Mathf.Min(currentPostItIndex + postItsPerPage, spawnedSequencialInfoCards.Count);
        for (int i = currentPostItIndex; i < endPostItIndex; i++)
        {
            if (pauseCoroutine)
            {
                continueCoroutine = false;
                currentPostItIndex = i; // Save the current index to continue from here later
                yield break; // Stop coroutine execution here
            }
            pageLoaded = false;
            spawnedSequencialInfoCards[i].gameObject.SetActive(true);
            spawnedSequencialInfoCards[i].ReadInfoCard();

            while (spawnedSequencialInfoCards[i].IsCardPlaying())
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(thoughtsContainerTransform);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            panelsLoaded++;
            if (panelsLoaded == panelsToLoad)
            {
                loadNextButton.GetComponent<Button>().interactable = (currentPage + 1) * postItsPerPage < spawnedSequencialInfoCards.Count; // Make sure the button is interactable
                nextButton.SetActive((currentPage + 1) * postItsPerPage >= spawnedSequencialInfoCards.Count); // Show nextButton if this is the last page
                nextButton.GetComponent<Button>().interactable = (currentPage + 1) * postItsPerPage >= spawnedSequencialInfoCards.Count; // Make sure the button is interactable
                loadPreviousButton.GetComponent<Button>().interactable = currentPage > 0; // Make sure the button is interactable
                loadedPages.Add(currentPage);

                if (currentPage == (spawnedSequencialInfoCards.Count - 1) / postItsPerPage)
                {
                    Debug.Log("Last card loaded. Next button is active and interactable.");
                }
                break;
            }
        }

        pageLoaded = true;
    }

    private void ResumeLifeCycle()
    {
        Debug.Log("ResumeLifeCycle called");
        if (pauseCoroutine)
        {
            pauseCoroutine = false;
            if (!continueCoroutine)
            {
                TryStartLifeCycle(); // Restart the coroutine to continue from where it left off
            }
        }
    }

    public void SelectCharacter(CharacterData data)
    {
        Debug.Log("SelectCharacter called");
        if (GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.ClueCollectionExtension>().GetThoughtsFor(data, true).Count == 0)
        {
            return;
        }

        Show();
    }

    public void PressNext()
    {
        Debug.Log("PressNext called");
        b_NextPressed = true;
        TryStopLifeCycle();
    }

    private void EnsureCarouselCounterIsBottomChild()
    {
        Debug.Log("EnsureCarouselCounterIsBottomChild called");
        carouselCounterContainer.transform.SetSiblingIndex(infoCardContainer.transform.childCount - 1);
        nextButton.transform.SetSiblingIndex(infoCardContainer.transform.childCount - 1);
    }
}
