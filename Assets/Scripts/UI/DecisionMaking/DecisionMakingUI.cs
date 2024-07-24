using System.Collections.Generic;
using Animation;
using Core;
using Data.ScenarioSettings;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace UI.DecisionMaking
{
    public class DecisionMakingUI : BaseUICanvas
    {
        [SerializeField] private List<TilesContainer> m_TilesContainers = new List<TilesContainer>(0);

        [SerializeField] private Button m_CheckButton = null;

        [SerializeField] private Button m_ButtonNext = null;

        [SerializeField] private RawImage m_CharacterImage = null;

        [SerializeField] private TextMeshProUGUI m_TitleText1 = null;

        [SerializeField] private TextMeshProUGUI m_TitleText2 = null;

        [SerializeField] private TextMeshProUGUI m_SubtitleLeftText = null;

        [SerializeField] private LocalizedString m_LocalizedString1 = null;

        [SerializeField] private LocalizedString m_LocalizedString2 = null;

        [SerializeField] private LocalizedString m_SubtitleLeft = null;

        private ScenarioSettings.DecisionMakingExtension m_Data = null;

        private int m_WrongAttempts = 0;

        // TODO: Tack this coroutine better
        private Coroutine m_Running = null;

        public bool IsDone() => m_TilesContainers.TrueForAll(t => t.Current().IsRightEntry == true) &&
                                m_Running == null && IsOnScreen() == false;

        [SerializeField] private UnityEvent m_OnDecisionMakingDone = null;

        private static DecisionMakingResult m_Result = null;

        public static DecisionMakingResult Result => m_Result;

        public override void Show()
        {
            base.Show();

            m_CheckButton.gameObject.SetActive(true);
            m_ButtonNext.gameObject.SetActive(false);

            m_Result = null;

            m_WrongAttempts = 0;

            m_Data = GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.DecisionMakingExtension>();

            m_TitleText1.text = string.Format(LocalizationManager.Instance.GetLocalizedValue(m_LocalizedString1), m_Data.Chatacter.GetName());
            m_TitleText2.text = string.Format(LocalizationManager.Instance.GetLocalizedValue(m_LocalizedString2), m_Data.Chatacter.GetName());
            m_SubtitleLeftText.text = string.Format(LocalizationManager.Instance.GetLocalizedValue(m_SubtitleLeft), m_Data.Chatacter.GetName());

            CharacterShowcase characterShowcase = m_Data.Chatacter.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);

            characterShowcase.GetComponent<DecisionMakingAnimations>().HandleDecisionMaking();

            m_CharacterImage.texture = characterShowcase.ImageTexture;

            for (int i = 0; i < m_TilesContainers.Count; i++)
            {
                m_TilesContainers[i].FeedData(m_Data, i);
            }

            MainGUI.Instance.MClueBookUI.ActivateClueBook();
            MainGUI.Instance.Button_SubtitleToggle.Show();
        }

        public override void Hide()
        {
            base.Hide();

            if (m_Running != null)
            {
                StopCoroutine(m_Running);
            }

            CharacterShowcase.ClearByOwner(this);

            m_TilesContainers.ForEach(container => { container.Clear(); });
            
            MainGUI.Instance.MSubtitlesUI.Hide();

            m_Data = null;

            MainGUI.Instance.Button_SubtitleToggle.Hide();
        }

        // Normally I wouldn't use Update, but we are sure that DecisionMaking UI go is
        // active only when needed.
        // Also, why on earth some Layout groups needs to be refreshed in order to work as intended?
        // Who knows...
        private void Update()
        {
            foreach (var container in m_TilesContainers) 
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(container.GetComponent<RectTransform>());
            }
        }

        public void CheckAnswer()
        {
            foreach (var tilesContainer in m_TilesContainers)
            {
                if (tilesContainer.Current().IsRightEntry == false)
                {
                    ScenarioSettings.DecisionMakingExtension.AttemptClipData attemptClipData =
                        tilesContainer.Current().AttemptClipOverride != null && tilesContainer.Current().AttemptClipOverride.Audio != null
                            ? tilesContainer.Current().AttemptClipOverride
                            : m_Data.WrongAttemptClips[m_WrongAttempts];

                    AudioManager.Instance.PlayDecisioMakingClip(attemptClipData.Audio);
                    MainGUI.Instance.MSubtitlesUI.ShowSubtitle(attemptClipData.SubTitles);

                    if (m_WrongAttempts == 1)
                    {
                        m_Running = StartCoroutine(Helpers.UI.COR_Cooldown(Consts.UI.k_DECISION_MAKING_END_CD, DisableInteraction, OnCdEnded));

                        void OnCdEnded()
                        {
                            m_TilesContainers.ForEach(t => t.ShowCorrectResult());

                            m_Running = StartCoroutine(Helpers.UI.COR_Cooldown(Consts.UI.k_DECISION_MAKING_END_CD, DisableInteraction, EnableInteraction_WrongAttempts));
                        }
                    }
                    else
                    {
                        m_Running = StartCoroutine(Helpers.UI.COR_Cooldown(attemptClipData.Audio.length, DisableInteraction, EnableInteraction));
                    }

                    m_WrongAttempts++;

                    return;
                }
            }

            m_Running = StartCoroutine(Helpers.UI.COR_Cooldown(Consts.UI.k_DECISION_MAKING_END_CD, DisableInteraction, EndDecisionMaking));

            void DisableInteraction()
            {
                m_CheckButton.gameObject.SetActive(false);

                foreach (var tilesContainer in m_TilesContainers)
                {
                    tilesContainer.ShowResult();

                    tilesContainer.DisableInteraction();
                }
            }

            void EnableInteraction()
            {
                m_CheckButton.gameObject.SetActive(true);

                foreach (var tilesContainer in m_TilesContainers)
                {
                    tilesContainer.ResetViz();

                    tilesContainer.EnableInteraction();
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

        public void EndDecisionMaking()
        {
            m_Result = new DecisionMakingResult(m_TilesContainers);

            MainGUI.Instance.MClueBookUI.DeactivateClueBook();

            m_OnDecisionMakingDone?.Invoke();
        }

        public class DecisionMakingResult
        {
            private List<(int tileIndex, int entryIndex)> m_TilesResult = new List<(int tileIndex, int entryIndex)>(0);

            private List<string> m_ResultTexts = new List<string>(0);

            public List<(int tileIndex, int entryIndex)> TilesResult => m_TilesResult;

            public List<string> ResultTexts => m_ResultTexts;

            public DecisionMakingResult(List<TilesContainer> tilesContainers)
            {
                foreach (var tilesContainer in tilesContainers)
                {
                    tilesContainer.Current().GetText();

                    m_TilesResult.Add((tilesContainer.TileIndex, tilesContainer.EntryIndex));

                    m_ResultTexts.Add(tilesContainer.Current().GetText());
                }
            }
        }
    }
}