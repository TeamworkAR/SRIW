using Animation;
using Data.ScenarioSettings;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class DisclaimerPanel : BaseUICanvas
{
    [SerializeField] private TextMeshProUGUI text = null;
    [SerializeField] private Image textBackground = null;
    [SerializeField] private RawImage characterImage = null;
    [SerializeField] private VerticalLayoutGroup layoutGroup;
    [SerializeField] private int defaultLeftPadding;

    private bool basicInfoEnded = false;
    private int noBackgroundLeftPadding = 5;

    public bool IsDone() => basicInfoEnded;


    public void ShowText(DisclaimerPanelDataWrapper wrapper)
    {
        basicInfoEnded = false;

        base.Show();

        if (string.IsNullOrEmpty(wrapper.DisclaimerPanelData.InfoString.GetLocalizedString()))
            text.gameObject.SetActive(false);
        else
        {
            text.gameObject.SetActive(true);
            text.text = wrapper.DisclaimerPanelData.InfoString.GetLocalizedString();
        }

        layoutGroup.padding.left = wrapper.DisclaimerPanelData.ShowTextBackground ? defaultLeftPadding : noBackgroundLeftPadding;

        textBackground.enabled = wrapper.DisclaimerPanelData.ShowTextBackground;

        CharacterShowcase characterShowcase = wrapper.DisclaimerPanelData.Owner.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);

        if (characterShowcase == null)
        {
            characterImage.enabled = false;
        }
        else
        {
            characterShowcase.GetComponent<ThoughtCollectionAnimations>().HandleThoughtCollection();
            characterImage.texture = characterShowcase.ImageTexture;
        }
    }

    public override void Show()
    {
        base.Show();

        basicInfoEnded = false;
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void EndInfoPanel()
    {
        basicInfoEnded = true;

        Hide();
    }
}
