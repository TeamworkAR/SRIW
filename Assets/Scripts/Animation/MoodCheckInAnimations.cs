using Core;
using UI.MoodCheckIn;
using UnityEngine;

namespace Animation
{
    [RequireComponent(typeof(MoodController))]
    public class MoodCheckInAnimations : MonoBehaviour
    {
        private Animator m_Animator = null;
        
        private MoodController m_MoodController = null;

        private void Awake()
        {
            m_MoodController = GetComponent<MoodController>();

            m_Animator = GetComponentInChildren<Animator>();
        }

        public void HandleMood(MoodCheckInUI.Mood mood)
        {
            m_Animator.runtimeAnimatorController = mood.ControllerOverride;
         
            m_Animator.SetTrigger(Consts.Animation.k_TRIGGER_CHARACTERSHOWCASE_MOODCHECKIN);
            
            m_MoodController.HandleMoodManual(mood.FacialExpressionsMood);
        }

        public void HandleSelected(bool selected)
        { 
            // m_Animator.SetBool(Consts.Animation.k_BOOL_CHARACTERSHOWCASE_MOODCHECKIN_ISSELECTED, selected);   
        }
    }
}