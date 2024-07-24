using Managers;
using TMPro;
using UnityEngine;

namespace UI.Smartphone
{
    public class SocialMediaPostSmartPhoneChainEntryDisplay : MonoBehaviour 
    {
        [SerializeField] private TextMeshProUGUI m_CharacterNameTextContainer = null;

        [SerializeField] private TextMeshProUGUI m_PostTextContainer = null;

        public void FeedData(SocialMediaPostSmartPhoneChainEntry data)
        {
            m_CharacterNameTextContainer.text = data.Data.GetName();

            m_PostTextContainer.text = LocalizationManager.Instance.GetLocalizedValue(data.PostText);
        }
    }
}