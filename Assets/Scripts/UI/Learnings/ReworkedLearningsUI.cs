using static UI.Learnings.ReworkedLearningsUI;

namespace UI.Learnings
{
    public class ReworkedLearningsUI : PanelContainer<ReworkedLearningsUIPanel> 
    {
        private bool b_LearningsEnded = false;

        public override bool IsDone() => base.IsDone() && b_LearningsEnded;

        protected override void OnShowStart()
        {
            base.OnShowStart();

            b_LearningsEnded = false;
        }

        public void EndLearnings()
        {
            b_LearningsEnded = true;

            Hide();
        }

        public class ReworkedLearningsUIPanel : BaseUICanvas { }
    }
}