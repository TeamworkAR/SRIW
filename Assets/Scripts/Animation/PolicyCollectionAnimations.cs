using Core;
using UnityEngine;

namespace Animation
{
    public class PolicyCollectionAnimations : MonoBehaviour
    {
        private Animator m_Animator = null;

        private void Awake()
        {
            m_Animator = GetComponentInChildren<Animator>();
        }

        public void HandlePolicyCollection()
        {
            m_Animator.SetTrigger(Consts.Animation.k_TRIGGER_CHARACTERSHOWCASE_POLICYCOLLECTION);
        }
    }
}
