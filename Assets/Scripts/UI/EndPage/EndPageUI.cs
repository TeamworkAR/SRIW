using Animation;
using Data.ScenarioSettings;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EndPage
{
    public class EndPageUI : BaseUICanvasGroup
    {
        [SerializeField] private RawImage m_CharacterImage = null;

        protected override void OnShowStart()
        {
            base.OnShowStart();
            
            var extension = GameManager.Instance.ScenarioSettings.GetExtension<EndPageExtension>();

            CharacterShowcase characterShowcase = extension.Character.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.HalfBody);

            characterShowcase.GetComponent<EndAnimations>().HandleEnd();

            characterShowcase.GetComponent<MoodController>().HandleMoodManual(extension.Mood);
            
            m_CharacterImage.texture = characterShowcase.ImageTexture;
        }
        
        protected override void OnHideCompleted()
        {
            CharacterShowcase.ClearByOwner(this);
            
            base.OnHideCompleted();
        }

        public void OnRequestEnd()
        {
            // TODO: Implement ScormAPI

            ScormManager.Instance.FinishCourse();
        }
    }
}