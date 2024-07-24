using Core;
using UnityEngine;

namespace Animation
{
    public class DecisionMakingAnimations : MonoBehaviour
    {
        private Animator m_Animator = null;

        private void Awake()
        {
            m_Animator = GetComponentInChildren<Animator>();
        }

        public void HandleDecisionMaking ()
        {
            m_Animator.SetTrigger(Consts.Animation.k_TRIGGER_CHARACTERSHOWCASE_DECISIONMAKING);
        }
    }
}
