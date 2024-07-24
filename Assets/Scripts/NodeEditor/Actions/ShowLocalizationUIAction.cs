using System;
using UI;

namespace NodeEditor.Actions
{
    [Serializable]
    public class ShowLocalizationUIAction : Action
    {
        public override void Execute() => MainGUI.Instance.MLocalizationUI.Show();
    }
}