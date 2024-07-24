using Animation;
using Core;
using Data.CharacterData;
using Data.ScenarioSettings;
using Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UI.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicCarouselPanel : BaseUICanvas
{
    [SerializeField] private RawImage image = null;

    [SerializeField] private TextMeshProUGUI title = null;
    [SerializeField] private TextMeshProUGUI content = null;

    [SerializeField] private Button nextButton = null;

    [SerializeField] private CarouselCounterContainer carouselCounterContainer = null;

    private ScenarioSettings.BasicCarouselPanelExtension extension = null;

    private Helpers.UI.CyclingList<ScenarioSettings.BasicCarouselPanelExtension.BasicCarouselEntry> entries = null;

    private bool basicCarouselEnded = false;

    public bool IsDone() => basicCarouselEnded;

    public void Show(CarouselPanelDataWrapper wrapper)
    {
        basicCarouselEnded = false;

        base.Show();

        entries = new Helpers.UI.CyclingList<ScenarioSettings.BasicCarouselPanelExtension.BasicCarouselEntry>(wrapper.SequencialInfoCardData.Entries);

        carouselCounterContainer.Init(entries.Count);

        ShowContent();

        nextButton.gameObject.SetActive(false);
    }

    public override void Show()
    {
        base.Show();

        extension = GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.BasicCarouselPanelExtension>();

        entries = new Helpers.UI.CyclingList<ScenarioSettings.BasicCarouselPanelExtension.BasicCarouselEntry>(extension.Entries);

        carouselCounterContainer.Init(entries.Count);

        ShowContent();

        nextButton.gameObject.SetActive(false);
    }

    public override void Hide()
    {
        carouselCounterContainer.Dispose();

        currentCharacter = null;

        CharacterShowcase.ClearByOwner(this);

        base.Hide();
    }

    private CharacterData currentCharacter = null;

    private void ShowContent()
    {
        if (ReferenceEquals(currentCharacter, entries.GetCurrent().Character) == false)
        {
            CharacterShowcase.ClearByOwner(this);

            currentCharacter = entries.GetCurrent().Character;

            CharacterShowcase showcase = entries.GetCurrent().Character.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);

            showcase.GetComponent<LearningsAnimations>().HandleLearnings(); //TODO: NEW ANIMATION TRIGGERS FOR GENERIC STUFF

            image.texture = showcase.ImageTexture;
        }

        title.text = entries.GetCurrent().Title.GetLocalizedString();
        content.text = entries.GetCurrent().Content.GetLocalizedString();

        carouselCounterContainer.Select(entries.Idx);

        nextButton.gameObject.SetActive(entries.IsLast);
    }

    public void Next()
    {
        entries.Next();

        ShowContent();
    }

    public void Previous()
    {
        entries.Previous();

        ShowContent();
    }

    public void EndCarouselPanel()
    {
        basicCarouselEnded = true;

        Hide();
    }
}
