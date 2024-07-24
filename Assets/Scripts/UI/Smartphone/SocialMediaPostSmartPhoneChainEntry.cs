using System;
using Data.CharacterData;
using UnityEngine;
using UnityEngine.Localization;

namespace UI.Smartphone
{
    [Serializable]
    public sealed class SocialMediaPostSmartPhoneChainEntry : SmartPhoneChainEntry
    {
        [SerializeField] private SocialMediaPostSmartPhoneChainEntryDisplay m_Template = null;

        [SerializeField] private CharacterData m_Data = null;

        [SerializeField] private LocalizedString m_PostText = null;

        public CharacterData Data => m_Data;
        public LocalizedString PostText => m_PostText;

        public override void Create(SmartphoneUI smartphoneUI)
        {
            SocialMediaPostSmartPhoneChainEntryDisplay instance = UnityEngine.Object.Instantiate(m_Template, smartphoneUI.EntriesContainer);

            instance.FeedData(this);
        }
    }
}