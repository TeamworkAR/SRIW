using System;
using Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animation
{
    public class ContextSettingsAnimations : MonoBehaviour
    {
        private Animator m_Animator = null;
        
        private void Awake()
        {
            m_Animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            m_Animator.SetFloat(Consts.Animation.k_CONTEXTSETTINGSIDLEOFFSET, Random.Range(0f,1f));
        }

        public void HandleContextSettings()
        {
            m_Animator.SetTrigger(Consts.Animation.k_TRIGGER_CHARACTERSHOWCASE_CONTEXTSETTINGS);
        }
    }
}