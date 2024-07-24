using Data.ScenarioSettings;
using Managers;

namespace UI.ClueCollection
{
    public class ClueCollectionUI : PanelContainer<BaseUICanvas>
    {
        protected override void OnShowCompleted()
        {
            base.OnShowCompleted();
            
            MainGUI.Instance.MClueBookUI.ActivateClueBook();
        }

        protected override void OnHideCompleted()
        {
            base.OnHideCompleted();
            
            MainGUI.Instance.MClueBookUI.DeactivateClueBook();
        }

        public override bool IsDone()
        {


            return base.IsDone() &&
                   IsOnScreen() == false &&
                   AreClueUnlocked();
        }

        private bool AreClueUnlocked()
        {
            var extension = GameManager.Instance.ScenarioSettings
                .GetExtension<ScenarioSettings.ClueCollectionExtension>();
            
            return extension.AreAllPoliciesUnlocked() &&
                   extension.LeftGroupCharacters.TrueForAll(c => extension.AreAllThoughtsUnlockedFor(c)) &&
                   extension.RightGroupCharacters.TrueForAll(c => extension.AreAllThoughtsUnlockedFor(c));
        }

        public void TryChangePanel(BaseUICanvas panel)
        {
            if (AreClueUnlocked())
            {
                Hide();
            }
            else
            {
                ChangePanel(panel);
            }
        }
    }
}