using System;
using Data.CharacterData;
using UnityEngine;
using UnityEngine.Localization;
using Object = UnityEngine.Object;

namespace UI.Smartphone
{
    [Serializable]
    public class TextSmartPhoneChainEntry : SmartPhoneChainEntry
    {
        [SerializeField] private TextSmartphoneChainEntryDisplay m_Template = null;
        
        [SerializeField] private CharacterData m_Character = null;
        
        [SerializeField] private LocalizedString m_Text = null;

        public CharacterData Character => m_Character;

        public LocalizedString Text => m_Text;

        public override void Create(SmartphoneUI smartphoneUI)
        {
            TextSmartphoneChainEntryDisplay instance = Object.Instantiate(m_Template, smartphoneUI.EntriesContainer);
            instance.FeedData(this);
            instance.GetComponentInChildren<AccessibleLabel>().Select();
        }
    }
}