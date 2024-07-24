using Animation;
using Data.ScenarioSettings;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UI.Learnings.ReworkedLearningsUI;

namespace UI.Learnings
{
    public class ReworkedLearningsUIPresentationPanel : ReworkedLearningsUIPanel 
    {
        [SerializeField] private TextMeshProUGUI m_Title = null;
        [SerializeField] private TextMeshProUGUI m_Text = null;
        [SerializeField] private TextMeshProUGUI m_ButtonText = null;

        [SerializeField] private RawImage m_Image = null;

        [SerializeField] private VerticalLayoutGroup m_VerticalLayout = null;

        public override void Show()
        {
            base.Show();

            CharacterShowcase showcase =
                GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.LearningsExtension>()
                    .LearningsCharacter.ShowcaseTemplate.GetInstance(this,
                        CharacterShowcase
                            .CameraPositions.HalfBody);

            showcase.GetComponent<LearningsAnimations>().HandleLearnings();

            m_Text.text = LocalizationManager.Instance.GetLocalizedValue(GameManager.Instance.ScenarioSettings.GetExtension<ScenarioSettings.LearningsExtension>().LearningsIntro);

            m_Image.texture = showcase.ImageTexture;

            LayoutRebuilder.ForceRebuildLayoutImmediate(m_VerticalLayout.GetComponent<RectTransform>());
        }

        public override void Hide()
        {
            base.Hide();

            CharacterShowcase.ClearByOwner(this);
        }
    }
}