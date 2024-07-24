using System;
using UI;
using UI.PostDialogueCarousel;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class ShowBasicSplashScreenUIAction : Action
    {
        [SerializeField] private BasicSplashScreenPanel.BasicSplashScreenData data = null;
        
        public override void Execute()
        {
            MainGUI.Instance.BasicSplashScreenPanel.Show(data);
        }
    }
}