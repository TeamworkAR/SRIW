using Core;
using UnityEngine;

namespace Animation
{
    public class ThoughtCollectionAnimations : MonoBehaviour
    {
        private Animator m_Animator = null;

        private void Awake()
        {
            m_Animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            m_Animator.SetFloat(Consts.Animation.k_THOUGHTSCOLLECTIONOFFSET, Random.Range(0f, 1f));
        }


        public void HandleThoughtCollection()
        {
            m_Animator.SetTrigger(Consts.Animation.k_TRIGGER_CHARACTERSHOWCASE_THOUGHTCOLLECTION);
        }
    }
}
