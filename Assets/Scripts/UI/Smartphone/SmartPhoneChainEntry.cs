using System;
using Managers;
using UnityEngine;

namespace UI.Smartphone
{
    [Serializable]
    public abstract class SmartPhoneChainEntry
    {
        [SerializeField] private AudioClip m_Sfx = null;
        
        public abstract void Create(SmartphoneUI smartphoneUI);

        public void DoSfx()
        {
            if (m_Sfx == null)
            {
                return;
            }
            
            AudioManager.Instance.DoSfx(m_Sfx);
        }
    }
}