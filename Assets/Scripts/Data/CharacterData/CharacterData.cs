using DialogueSystem;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.Localization;

namespace Data.CharacterData
{
    [CreateAssetMenu(menuName = "Data/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        [SerializeField] private CharacterShowcase m_ShowcaseTemplate = null;
        [SerializeField] private DialogueCharacter m_DialogueTemplate = null;
        [SerializeField] private LocalizedString characterName;
        [SerializeField] private LocalizedString characterInfo;

        public CharacterShowcase ShowcaseTemplate => m_ShowcaseTemplate;

        public DialogueCharacter DialogueTemplate => m_DialogueTemplate;

        public string GetName() => LocalizationManager.Instance.GetLocalizedValue(characterName);
        
        public string GetDescription() => LocalizationManager.Instance.GetLocalizedValue(characterInfo);
    }
}