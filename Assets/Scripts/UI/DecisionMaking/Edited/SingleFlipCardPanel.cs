using Animation;
using Core;
using Data.ScenarioSettings;
using Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class SingleFlipCardPanel : BaseUICanvas
{
    [SerializeField] private FlipCardSimple card = null;

    [SerializeField] private Button m_ButtonNext = null;

    [SerializeField] private RawImage m_CharacterImage = null;

    [SerializeField] private TextMeshProUGUI titleText = null;

    [SerializeField] private GameObject titleOBj = null;

    [SerializeField] private TextMeshProUGUI subTitleText = null;

    [SerializeField] private GameObject subTitleOBj = null;


    [SerializeField] private TextMeshProUGUI infoText = null;

    [SerializeField] private GameObject infoTextOBj = null;

    private SingleFlipPanelData m_Data = null;

    // TODO: Tack this coroutine better
    private Coroutine m_Running = null;

    private bool endedDecision = false;

    // public bool IsDone() => cards.TrueForAll(t => t.IsCorrectlySelectedOrUnselected == true) && m_Running == null && IsOnScreen() == false;
    public bool IsDone() => endedDecision;

    public void ShowWithWrapperData(SingleFlipCardDataWrapper singleFlipCardDataWrapper)
    {
        endedDecision = false;

        this.gameObject.SetActive(true);
        base.Show();

        m_ButtonNext.gameObject.SetActive(false);

       // m_WrongAttempts = 0;

        m_Data = singleFlipCardDataWrapper.SingleFlipCardData;

        /*
        circleText_Top.text = localizedString_CircleTop.GetLocalizedString();
        circleText_Bot.text = localizedString_CircleBot.GetLocalizedString();
        lowerText.text = localizedString_Lower.GetLocalizedString();
        */

        CharacterShowcase characterShowcase = m_Data.Chatacter.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);

        characterShowcase.GetComponent<DecisionMakingAnimations>().HandleDecisionMaking();

        m_CharacterImage.texture = characterShowcase.ImageTexture;

        card.ResetViz();

        card.FeedData(m_Data.Tile);

        if (m_Data.TitleString != null)
        {
            titleText.text = m_Data.TitleString.GetLocalizedString();
            titleOBj.SetActive(true);
        }
        else
        {
            titleOBj.SetActive(false);
        }


        if (!m_Data.SubTitleString.IsEmpty)
        {
            subTitleText.text = m_Data.SubTitleString.GetLocalizedString();
            subTitleOBj.SetActive(true);
        }
        else
        {
            subTitleOBj.SetActive(false);
        }


        if (m_Data.InfoString != null)
        {
            infoText.text = m_Data.InfoString.GetLocalizedString();
            infoTextOBj.SetActive(true);
        }
        else
        {
            infoTextOBj.SetActive(false);
        }

    }

    /* public override void Show()
     {
         this.gameObject.SetActive(true);
         base.Show();

         m_CheckButton.gameObject.SetActive(true);
         m_ButtonNext.gameObject.SetActive(false);

         m_WrongAttempts = 0;

         m_Data = GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.DecisionMakingExtension>();

         circleText_Top.text = localizedString_CircleTop.GetLocalizedString();
         circleText_Bot.text = localizedString_Lower.GetLocalizedString();
         lowerText.text = localizedString_Lower.GetLocalizedString();

         CharacterShowcase characterShowcase = m_Data.Chatacter.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);

         characterShowcase.GetComponent<DecisionMakingAnimations>().HandleDecisionMaking();

         m_CharacterImage.texture = characterShowcase.ImageTexture;

         for (int i = 0; i < m_Data.Tiles.Count; i++)
         {
             cards[i].ResetViz();

             cards[i].FeedData();
         }

         //forcing every tile to be enabled when the decision making panel is shown
         foreach (var tile in cards)
         {
             tile.enabled = true;
         }
     }*/

    public override void Hide()
    {
        base.Hide();

        if (m_Running != null)
        {
            StopCoroutine(m_Running);
        }

        CharacterShowcase.ClearByOwner(this);

        m_Data = null;
        this.gameObject.SetActive(false);
    }

   /*
    public void CheckAnswer()
    {
        foreach (var tile in cards)
        {
            if (!tile.IsCorrectlySelectedOrUnselected)
            {
                float cooldownTime = 2f;

                if (m_Data.WrongAttemptClips.Count > m_WrongAttempts)
                {
                    ScenarioSettings.DecisionMakingExtension.AttemptClipData attemptClipData =
                    tile.FlipCardData.AttemptClipOverride != null && tile.FlipCardData.AttemptClipOverride.Audio != null
                       ? tile.FlipCardData.AttemptClipOverride
                       : m_Data.WrongAttemptClips[m_WrongAttempts];


                    AudioManager.Instance.PlayDecisioMakingClip(attemptClipData.Audio);
                    MainGUI.Instance.MSubtitlesUI.ShowSubtitle(attemptClipData.SubTitles);

                    cooldownTime = attemptClipData.Audio.length;
                }

                if (m_WrongAttempts == 1)
                {
                    m_Running = StartCoroutine(Helpers.UI.COR_Cooldown(Consts.UI.k_DECISION_MAKING_END_CD, () => DisableInteraction(false), OnCdEnded));

                    void OnCdEnded()
                    {
                        // Right answer clip was removed from scripts
                        // AudioManager.Instance.PlayDecisioMakingClip(m_Data.RightAttemptClip.Audio);
                        // MainGUI.Instance.MSubtitlesUI.ShowSubtitle(m_Data.RightAttemptClip.SubTitles);

                        cards.ForEach(t => t.ShowResult());

                        m_Running = StartCoroutine(Helpers.UI.COR_Cooldown(Consts.UI.k_DECISION_MAKING_END_CD, () => DisableInteraction(false), EnableInteraction_WrongAttempts));
                    }
                }
                else
                {
                    m_Running = StartCoroutine(Helpers.UI.COR_Cooldown(cooldownTime, () => DisableInteraction(false), EnableInteraction));
                }

                m_WrongAttempts++;

                return;
            }
        }
   

        // Right answer clip was removed from scripts
        // AudioManager.Instance.PlayDecisioMakingClip(m_Data.RightAttemptClip.Audio);
        // MainGUI.Instance.MSubtitlesUI.ShowSubtitle(m_Data.RightAttemptClip.SubTitles);

        m_Running = StartCoroutine(Helpers.UI.COR_Cooldown(Consts.UI.k_DECISION_MAKING_END_CD, () => DisableInteraction(true), () => { EnableInteraction(); EndDecisionMaking(); }));

        void DisableInteraction(bool finalAnswer)
        {
            m_CheckButton.gameObject.SetActive(false);

            foreach (var tile in cards)
            {
                if (finalAnswer)
                    tile.ShowResult();
                tile.enabled = false;

            }
        }

        void EnableInteraction()
        {
            m_CheckButton.gameObject.SetActive(true);

            foreach (var tile in cards)
            {
                tile.ResetViz();

                tile.enabled = true;

            }

            m_Running = null;

            MainGUI.Instance.MSubtitlesUI.ClearSubtitles();
        }

        void EnableInteraction_WrongAttempts()
        {
            m_CheckButton.gameObject.SetActive(false);
            m_ButtonNext.gameObject.SetActive(true);

            m_Running = null;

            MainGUI.Instance.MSubtitlesUI.ClearSubtitles();
        }
    }
     */
    public void EndSimpleFlipPanel()
    {
        Hide();
        endedDecision = true;
    }
}
