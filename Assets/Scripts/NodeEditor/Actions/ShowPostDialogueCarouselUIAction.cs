using System;
using UI;
using UI.PostDialogueCarousel;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class ShowPostDialogueCarouselUIAction : Action
    {
        [SerializeField] private PostDialogueCarouselUI.PostDialogueCarouselData m_Data = null;
        
        public override void Execute()
        {
            MainGUI.Instance.MPostDialogueCarouselUI.Show(m_Data);
        }
    }
}