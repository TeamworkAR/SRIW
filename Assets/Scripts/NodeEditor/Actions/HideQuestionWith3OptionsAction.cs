using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class HideQuestionWith3OptionsAction : Action
    {
        public override void Execute()
        {
            MainGUI.Instance.QuestionWith3OptionPanel.Interrupt();
        }
    }
}