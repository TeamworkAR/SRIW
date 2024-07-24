using Data.ScenarioSettings;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Animation;

public class QuestionWith4OptionsPanel : BaseUICanvas
{
    [Header("Response buttons")]
    [SerializeField] private List<WrongAnswerButton> responseButtons;

    [Header("UI components")]
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text itDependsTitle;
    [SerializeField] private TMP_Text itDependsText;
    [SerializeField] private RawImage characterImage;
    [SerializeField] private GameObject questionUI;
    [SerializeField] private GameObject correctAnswerUI;

    private bool uiEnded = false;
    public bool IsDone() => uiEnded;

    /// <summary>
    /// Called by the Show action
    /// </summary>
    /// <param name="wrapper"></param>
    public void ShowText(QuestionWith3OptionsDataWrapper wrapper)
    {
        Debug.Log("#QuestionWith3Options - Starting...");

        uiEnded = false;

        base.Show();

        questionUI.SetActive(true);
        correctAnswerUI.SetActive(true);

        //Question text
        if (!string.IsNullOrEmpty(wrapper.data.QuestionText.GetLocalizedString()))
        {
            questionText.text = wrapper.data.QuestionText.GetLocalizedString();
        }

        //Correct response title
        if (!string.IsNullOrEmpty(wrapper.data.CorrectResponseTitle.GetLocalizedString()))
        {
            itDependsTitle.text = wrapper.data.CorrectResponseTitle.GetLocalizedString();
        }

        //Correct response text
        if (!string.IsNullOrEmpty(wrapper.data.CorrectResponseText.GetLocalizedString()))
        {
            itDependsText.text = wrapper.data.CorrectResponseText.GetLocalizedString();
        }
        correctAnswerUI.SetActive(false);

        //Reset button states
        for (int i = 0; i < responseButtons.Count; i++)
        {
            responseButtons[i].ResetCorrectResponse();
        }

        //Define coorect response
        responseButtons[wrapper.data.CorrectResponseIndex].SetAsCorrectResponse(ShowCorrectResponse);

        //Show character
        if (wrapper.data.Owner == null)
        {
            characterImage.enabled = false;
            return;
        }

        CharacterShowcase characterShowcase = wrapper.data.Owner.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);
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

    private void ShowCorrectResponse()
    {
        questionUI.SetActive(false);
        correctAnswerUI.SetActive(true);
    }
}