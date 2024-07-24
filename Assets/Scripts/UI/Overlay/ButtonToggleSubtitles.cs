using UI.Subtitles;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Overlay
{
    public class ButtonToggleSubtitles : BaseUICanvas
    {
        [SerializeField] private Image m_Icon = null;
        
        [SerializeField] private Sprite m_SubtitlesOnButton = null;
        [SerializeField] private Sprite m_SubtitlesOffButton = null;

        [SerializeField] private SubtitlesUI m_SubtitlesUI = null;

        public void Toggle() => m_Icon.sprite = m_SubtitlesUI.SubtitlesOn ? m_SubtitlesOnButton : m_SubtitlesOffButton;
    }
}