using Animation;
using Data.ScenarioSettings;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class BasicInfoWithTitlePanel : BaseUICanvas
{
    [SerializeField] private TextMeshProUGUI titleText = null;
    [SerializeField] private TextMeshProUGUI text = null;
    [SerializeField] private Image textBackground = null;
    [SerializeField] private VerticalLayoutGroup layoutGroup;
    [SerializeField] private int defaultLeftPadding;

    private bool basicInfoEnded = false;
    private int noBackgroundLeftPadding = 5;

    public bool IsDone() => basicInfoEnded;


    public void ShowText(BasicInfoWithTitlePanelDataWrapper wrapper)
    {
        basicInfoEnded = false;

        base.Show();

        if (string.IsNullOrEmpty(wrapper.BasicInfoWithTitlePanelData.InfoString.GetLocalizedString()))
            text.gameObject.SetActive(false);
        else
        {
            text.gameObject.SetActive(true);
            text.text = wrapper.BasicInfoWithTitlePanelData.InfoString.GetLocalizedString();

        }

        if (string.IsNullOrEmpty(wrapper.BasicInfoWithTitlePanelData.TitleString.GetLocalizedString()))
            titleText.gameObject.SetActive(false);
        else
        {
            titleText.gameObject.SetActive(true);
            titleText.text = wrapper.BasicInfoWithTitlePanelData.TitleString.GetLocalizedString();
        }


        //layoutGroup.padding.left = wrapper.BasicInfoPanelData.ShowTextBackground ? defaultLeftPadding : noBackgroundLeftPadding;
        //
        //textBackground.enabled = wrapper.BasicInfoPanelData.ShowTextBackground;

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
