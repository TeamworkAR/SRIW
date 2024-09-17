using Core;
using UI.AlternativeDialogue;
using UI.ClueCollection;
using UI.ClueCollection.ClueBook;
using UI.ClueCollection.ThoughtsCollection;
using UI.Conclusion;
using UI.ContextSettings;
using UI.DecisionMaking;
using UI.DialogueChoice;
using UI.EndPage;
using UI.Learnings;
using UI.Localization;
using UI.MoodCheckIn;
using UI.Overlay;
using UI.PostDialogueCarousel;
using UI.ScreenCover;
using UI.Smartphone;
using UI.Subtitles;
using UnityEngine;

namespace UI
{
    public class MainGUI : SingletonBehaviour<MainGUI>
    {
        [SerializeField] private BackgroundUI m_BackgroundUI = null;

        [SerializeField] private ContextSettingsUI m_ContextSettingsUI = null;

        [SerializeField] private MoodCheckInUI m_MoodCheckInUI = null;

        [SerializeField] private ReworkedLearningsUI m_LearningsUI = null;

        [SerializeField] private DecisionMakingPanelContainer m_DecisionMakingUI = null;

        [SerializeField] private ClueCollectionUI m_ClueCollectionUI = null;

        [SerializeField] private ScreenCoverUI m_ScreenCoverUI = null;

        [SerializeField] private DialogueChoiceUI m_DialogueChoiceUI = null;

        [SerializeField] private ConclusionUI m_ConclusionsUI = null;

        [SerializeField] private EndPageUI m_EndPageUI = null;

        [SerializeField] private SubtitlesUI m_SubtitlesUI = null;

        [SerializeField] private LocalizationUI m_LocalizationUI = null;

        [SerializeField] private ClueBookUI m_ClueBookUI = null;

        [SerializeField] private AlternativeDialogueUI m_AlternativeDialogueUI = null;

        [SerializeField] private PostDialogueCarouselUI m_PostDialogueCarouselUI = null;

        [SerializeField] private SmartphoneUI m_SmartphoneUI = null;

        [SerializeField] private ThoughtsDisplayPanel m_StandaloneThoughtsDisplay = null;

        [SerializeField] private StepsCountUI m_StepsCountUI = null;

        [SerializeField] private AccessibilityOverlay m_SceneDescriptionUI = null;

        [SerializeField] private ButtonToggleSubtitles m_Button_SubtitleToggle = null;
        [SerializeField] private ApplicationPauseManager m_Button_PauseToggle = null;

        [Header("Generic:")]
        [SerializeField] private BasicSplashScreenPanel basicSplashScreenPanel = null;

        [SerializeField] private BasicInfoPanel basicInfoPanelUI = null;

        [SerializeField] private BasicInfoWithTitlePanel basicInfoWithTitlePanelUI = null;

        [SerializeField] private BasicCarouselPanel basicCarouselPanelUI = null;

        [SerializeField] private SequencialInfoPanel sequencialInfoPanel = null;

        [SerializeField] private SequencialConclusionInfoPanel sequencialConclusionInfoPanel = null;

        [SerializeField] private SequencialInfoPostIt sequencialInfoPostIt = null;

        [SerializeField] private SequencialInfoPanelHorizontal sequencialInfoHorizontalPanel = null;

        [SerializeField] private SingleFlipCardPanel singleFlipPanel = null;

        [SerializeField] private DecisionMakingGridUI decisionMakingGridUI = null;

        [SerializeField] private DecisionMaking4GridUI decisionMaking4GridUI = null;

        [SerializeField] private QuestionWith3OptionsPanel questionWith3OptionPanel = null;

        [SerializeField] private QuestionWith4OptionsPanel questionWith4OptionPanel = null;

        [SerializeField] private BasicCharacterOnLeftPanel basicCharacterOnLeftPanel = null;

        [SerializeField] private DisclaimerPanel disclaimerPanel = null;
        public QuestionWith3OptionsPanel QuestionWith3OptionPanel => questionWith3OptionPanel;
        public QuestionWith4OptionsPanel QuestionWith4OptionPanel => questionWith4OptionPanel;
        [SerializeField] private Flip4CardsWithDoubleSideTextPanel flip4CardsWithDoubleSideTextPanel = null;
        public Flip4CardsWithDoubleSideTextPanel Flip4CardsWithDoubleSideTextPanel => flip4CardsWithDoubleSideTextPanel;

        [SerializeField] private ReminderPanel reminderPanel = null;
        public ReminderPanel ReminderPanel => reminderPanel;
        public BackgroundUI MBackgroundUI => m_BackgroundUI;

        public ContextSettingsUI MContextSettingsUI => m_ContextSettingsUI;

        public MoodCheckInUI MMoodCheckInUI => m_MoodCheckInUI;

        public ReworkedLearningsUI MLearningsUI => m_LearningsUI;

        public DecisionMakingPanelContainer MDecisionMakingUI => m_DecisionMakingUI;

        public ClueCollectionUI MClueCollectionUI => m_ClueCollectionUI;

        public ScreenCoverUI MScreenCoverUI => m_ScreenCoverUI;

        public DialogueChoiceUI MDialogueChoiceUI => m_DialogueChoiceUI;

        public ConclusionUI MConclusionsUI => m_ConclusionsUI;

        public EndPageUI MEndPageUI => m_EndPageUI;

        public SubtitlesUI MSubtitlesUI => m_SubtitlesUI;

        public LocalizationUI MLocalizationUI => m_LocalizationUI;

        public ClueBookUI MClueBookUI => m_ClueBookUI;

        public AlternativeDialogueUI MAlternativeDialogueUI => m_AlternativeDialogueUI;

        public PostDialogueCarouselUI MPostDialogueCarouselUI => m_PostDialogueCarouselUI;

        public SmartphoneUI MSmartphoneUI => m_SmartphoneUI;

        public ThoughtsDisplayPanel StandaloneThoughtsDisplay => m_StandaloneThoughtsDisplay;

        public StepsCountUI MStepsCountUI => m_StepsCountUI;
        public AccessibilityOverlay SceneDescription => m_SceneDescriptionUI;

        public ButtonToggleSubtitles Button_SubtitleToggle => m_Button_SubtitleToggle;
        public ApplicationPauseManager Button_PauseToggle => m_Button_PauseToggle;

        public BasicSplashScreenPanel BasicSplashScreenPanel => basicSplashScreenPanel;
        public BasicInfoPanel BasicInfoPanel => basicInfoPanelUI;
        public BasicInfoWithTitlePanel BasicInfoWithTitlePanel => basicInfoWithTitlePanelUI;
        public BasicCarouselPanel BasicCarouselPanel => basicCarouselPanelUI;
        public SequencialInfoPanel SequencialInfoPanel => sequencialInfoPanel;
        public SequencialConclusionInfoPanel SequencialConclusionInfoPanel => sequencialConclusionInfoPanel;
        public SequencialInfoPostIt SequencialInfoPostIt => sequencialInfoPostIt;
        public SequencialInfoPanelHorizontal SequencialInfoHorizontalPanel => sequencialInfoHorizontalPanel;
        public DecisionMakingGridUI DecisionMakingGridUI => decisionMakingGridUI;
        public DecisionMaking4GridUI DecisionMaking4GridUI => decisionMaking4GridUI;
        public SingleFlipCardPanel SingleFlipPanel => singleFlipPanel;
        public BasicCharacterOnLeftPanel BasicCharacterOnLeftPanel => basicCharacterOnLeftPanel;

        public DisclaimerPanel DisclaimerPanel => disclaimerPanel;
    }
}