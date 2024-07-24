using System;
using Data.CharacterData;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI.Smartphone
{
    [Serializable]
    public class ImageSmartPhoneChainEntry : SmartPhoneChainEntry
    {
        [SerializeField] private ImageSmartphoneChainEntryDisplay m_Template = null;
        
        [SerializeField] private CharacterData m_Character = null;
        
        [SerializeField] private Sprite m_Image = null;

        public CharacterData Character => m_Character;

        public Sprite Image => m_Image;

        public override void Create(SmartphoneUI smartphoneUI)
        {
            ImageSmartphoneChainEntryDisplay instance = Object.Instantiate(m_Template, smartphoneUI.EntriesContainer);
            instance.FeedData(this);
        }
    }
}