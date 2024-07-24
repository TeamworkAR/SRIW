using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    // TODO: DevSettings stub implementation. It should resemble ScenarioSettings i.e. use some Extension system (e.g. DevSettings.UI)
    [CreateAssetMenu(menuName = "Editor/" + nameof(DevSettings), fileName = nameof(DevSettings), order = - 1)]
    public class DevSettings : ScriptableObject
    {
        [SerializeField] private Color m_FoundationalGold = new Color();
        
        [SerializeField] private Color m_CorrectAnswerColor = Color.green;

        [SerializeField] private Color m_WrongAnswerColor = Color.red;

        [SerializeField] private Color m_UIGreen = new Color();

        [SerializeField] private Color m_CarouselCounterSelected = new Color();
        
        [SerializeField] private Color m_CarouselCounterUnselected = new Color();

        [SerializeField] private float m_PoliciesFixedReadTime = 4f;

        [Header("Buttons")] 
        [SerializeField] private Color m_NormalColor = new Color();
        [SerializeField] private Color m_SelectedColor = new Color();
        [SerializeField] private Color m_DisabledColor = new Color();
        [SerializeField] private Color m_ChevronsNormalColor = new Color();
        [SerializeField] private Color m_ChevronsDisabledColor = new Color();

        [Header("SmartphoneUI")] 
        [SerializeField] private float m_MessagesWaitTime = 4f;

        [Header("DecisionMakingTutorial")] 
        [SerializeField] private ScenarioSettings.ScenarioSettings.DecisionMakingExtension m_DecisionMakingTutorial = null;

        [Header("UI Transition")] 
        [SerializeField] private float m_BaseFadeDuration = 1f;
        [SerializeField] private float m_CharacterDisplayFadeDuration = 2f;
        
        public Color CorrectAnswerColor => m_CorrectAnswerColor;

        public Color WrongAnswerColor => m_WrongAnswerColor;

        public Color FoundationalGold => m_FoundationalGold;

        public float CharacterDisplayFadeDuration => m_CharacterDisplayFadeDuration;

        public Color UIGreen => m_UIGreen;

        public float PoliciesFixedReadTime => m_PoliciesFixedReadTime;

        public Color CarouselCounterSelected => m_CarouselCounterSelected;

        public Color CarouselCounterUnselected => m_CarouselCounterUnselected;

        public Color NormalColor => m_NormalColor;

        public Color HighlightedColor => m_FoundationalGold;

        public Color PressedColor => m_FoundationalGold;

        public Color SelectedColor => m_SelectedColor;

        public Color DisabledColor => m_DisabledColor;

        public Color ChevronsNormalColor => m_ChevronsNormalColor;

        public Color ChevronsDisabledColor => m_ChevronsDisabledColor;

        public float MessagesWaitTime => m_MessagesWaitTime;

        public ScenarioSettings.ScenarioSettings.DecisionMakingExtension DecisionMakingTutorial =>
            m_DecisionMakingTutorial;

        public float BaseFadeDuration => m_BaseFadeDuration;
    }
}