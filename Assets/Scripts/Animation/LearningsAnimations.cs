using Core;
using UnityEngine;

namespace Animation
{
    public class LearningsAnimations : MonoBehaviour
    {
        private Animator m_Animator = null;

        private void Awake()
        {
            m_Animator = GetComponentInChildren<Animator>();
        }

        public void HandleLearnings()
        {
            m_Animator.SetTrigger(Consts.Animation.k_TRIGGER_CHARACTERSHOWCASE_LEARNINGS);
        }
    }
}
