using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideQuestionWith4OptionsAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.QuestionWith4OptionPanel.Interrupt();
        }
    }
}