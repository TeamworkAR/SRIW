using Data.ScenarioSettings;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Animation;

public class ReminderPanel : BaseUICanvas
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private RawImage characterImage;

    private bool uiEnded = false;
    public bool IsDone() => uiEnded;

    /// <summary>
    /// Called by the Show action
    /// </summary>
    /// <param name="wrapper"></param>
    public void ShowText(ReminderDataWrapper wrapper)
    {
        Debug.Log("#QuestionWith3Options - Starting...");

        uiEnded = false;

        base.Show();

        nextButton.SetActive(false);

        messageText.text = wrapper.Data.Message.GetLocalizedString();

        StartCoroutine(ShowNextButton());

        //Show character
        if (wrapper.Data.Character == null)
        {
            characterImage.enabled = false;
            return;
        }

        CharacterShowcase characterShowcase = wrapper.Data.Character.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);
        if (characterShowcase == null)
        {
            characterImage.enabled = false;
        }
        else
        {
            characterImage.enabled = true;
            characterShowcase.GetComponent<ThoughtCollectionAnimations>().HandleThoughtCollection();
            characterImage.texture = characterShowcase.ImageTexture;
        }
    }

    IEnumerator ShowNextButton()
    {
        yield return new WaitForSeconds(6f);

        nextButton.SetActive(true);
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
