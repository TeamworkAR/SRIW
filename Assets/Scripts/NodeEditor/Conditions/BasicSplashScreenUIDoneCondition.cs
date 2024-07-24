using System;
using UI;

namespace NodeEditor.Conditions
{
    [Serializable]
    public class BasicSplashScreenUIDoneCondition : Condition
    {
        public override bool Evaluate() => MainGUI.Instance.BasicSplashScreenPanel.IsDone();
    }
}