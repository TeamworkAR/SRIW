using System;
using Animation;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.MoodCheckIn
{
    public class MoodCheckInDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RawImage m_Image = null;
        
        [SerializeField] private TextMeshProUGUI m_Text = null;
        
        [SerializeField] private Button m_Button = null;

        private MoodCheckInAnimations m_CharacterShowcaseAnimator = null;

        private MoodCheckInUI.Mood m_Mood = null;
        
        // TODO: Evaluate if it can make sense to make this field static and pass a Mood on its handlers.
        public Action OnMoodChosen = null;

        // TODO: Just using a flag for now, evaluate if a more elegant solution will be needed in the future.
        private bool b_Chosen = false;
        
        public void Show(CharacterShowcase showcase, MoodCheckInUI.Mood mood)
        {
            m_Mood = mood;
            
            m_Image.texture = showcase.ImageTexture;

            m_Text.text = m_Mood.Name;
            
            // TODO: Ewww
            m_CharacterShowcaseAnimator = showcase.GetComponent<MoodCheckInAnimations>();
            
            m_CharacterShowcaseAnimator.HandleMood(m_Mood);
        }

        public void HandlePressed()
        {
            b_Chosen = true;
            
            m_CharacterShowcaseAnimator.HandleSelected(true);
            
            OnMoodChosen?.Invoke();
        }

        public void DisableInteraction()
        {
            m_Button.interactable = false;

            // We want the chosen option to remain in its pressed colors, the other can remain in their disabled color
            if (b_Chosen == true)
            {
                // See Selectable.StartColorTween
                m_Button.targetGraphic.CrossFadeColor(m_Button.colors.pressedColor, 0f, true, true);
                
                // TODO: Change text color too. We also can set them in the button OnClick method because MoodCheckInDisplay are disposed at the end of MoodCheckIn phase.
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (m_Button.interactable == false)
            {
                return;
            }
            
            m_CharacterShowcaseAnimator.HandleSelected(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (m_Button.interactable == false)
            {
                return;
            }
            
            m_CharacterShowcaseAnimator.HandleSelected(false);
        }
    }
}