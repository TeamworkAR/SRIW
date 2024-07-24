using System;
using Animation;
using Data;
using Data.CharacterData;
using Data.ScenarioSettings;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ClueCollection.ThoughtsCollection
{
    public class ThoughtsCollectionCharacterButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private RawImage m_Image = null;

        public static event Action<CharacterData> OnCharacterSelected = null;

        private CharacterData m_CharacterData = null;

        private ScenarioSettings.ClueCollectionExtension m_Extension = null;
        
        public void Show(CharacterData characterData)
        {
            m_Extension = GameManager.Instance.ScenarioSettings
                .GetExtension<ScenarioSettings.ClueCollectionExtension>();
            
            m_CharacterData = characterData;

            CharacterShowcase characterShowcase = characterData.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.HalfBody);

            characterShowcase.GetComponent<ThoughtCollectionAnimations>().HandleThoughtCollection();

            m_Image.texture = characterShowcase.ImageTexture;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_Extension.AreAllThoughtsUnlockedFor(m_CharacterData))
            {
                return;
            }

            OnCharacterSelected?.Invoke(m_CharacterData);    
        }
        
        private void OnDestroy() => CharacterShowcase.ClearByOwner(this);
    }
}