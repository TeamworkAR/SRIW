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

public class DecisionMakingGridUI : BaseUICanvas
{
    [SerializeField] private List<FlipCard> cards = null;

    [SerializeField] private Button m_CheckButton = null;

    [SerializeField] private Button m_ButtonNext = null;

    [SerializeField] private RawImage m_CharacterImage = null;

    [SerializeField] private TextMeshProUGUI circleText_Top = null;

    [SerializeField] private TextMeshProUGUI circleText_Bot = null;

    [SerializeField] private TextMeshProUGUI lowerText = null;

    //[SerializeField] private LocalizedString localizedString_CircleTop = null;
    //
    //[SerializeField] private LocalizedString localizedString_CircleBot = null;
    //
    //[SerializeField] private LocalizedString localizedString_Lower = null;

    private DecisionMakingGridData m_Data = null;

    private int m_WrongAttempts = 0;

    // TODO: Tack this coroutine better
    private Coroutine m_Running = null;

    private bool endedDecision = false;

    // public bool IsDone() => cards.TrueForAll(t => t.IsCorrectlySelectedOrUnselected == true) && m_Running == null && IsOnScreen() == false;
    public bool IsDone() => endedDecision;

    public void ShowWithWrapperData(DecisionMakingGridDataWrapper decisionMakingGridDataWrapper)
    {
        endedDecision = false;

        this.gameObject.SetActive(true);
        base.Show();

        m_CheckButton.gameObject.SetActive(true);
        m_ButtonNext.gameObject.SetActive(false);

        m_WrongAttempts = 1;

        m_Data = decisionMakingGridDataWrapper.DecisionMakingGridData;


        circleText_Top.text = m_Data.CircleTopString.GetLocalizedString();
        circleText_Bot.text = m_Data.CircleBottomString.GetLocalizedString();
        lowerText.text = m_Data.LowerString.GetLocalizedString();

        if (m_Data.Chatacter != null)
        {
            CharacterShowcase characterShowcase = m_Data.Chatacter.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);

            characterShowcase.GetComponent<DecisionMakingAnimations>().HandleDecisionMaking();

            if (m_CharacterImage != null)
            {
                m_CharacterImage.enabled = true;
                m_CharacterImage.texture = characterShowcase.ImageTexture;
            }

            
        }
        else
            m_CharacterImage.enabled = false;
        

        for (int i = 0; i < m_Data.Tiles.Count; i++)
        {
            cards[i].ResetViz();

            cards[i].FeedData(m_Data.Tiles[i]);
        }

        //forcing every tile to be enabled when the decision making panel is shown
        foreach (var tile in cards)
        {
            tile.enabled = true;
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
            m_CheckButton.interactable = false;

            foreach (var tile in cards)
            {
                if (finalAnswer)
                    tile.ShowResult();
                tile.enabled = false;

            }
        }

        void EnableInteraction()
        {
            m_CheckButton.interactable = true;

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
            m_CheckButton.interactable = true;
            m_ButtonNext.gameObject.SetActive(true);

            m_Running = null;

            MainGUI.Instance.MSubtitlesUI.ClearSubtitles();
        }
    }

    public void EndDecisionMaking()
    {
        Hide();
        endedDecision = true;
    }
}
