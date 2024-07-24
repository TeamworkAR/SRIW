using System.Collections.Generic;
using Data.CharacterData;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ClueCollection.ClueBook
{
    public class ClueBookCharacterButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Image m_Background = null;
        
        [SerializeField] private Image m_Extension = null;

        [SerializeField] private TextMeshProUGUI m_Text = null;

        private CharacterData m_Character = null;

        private ClueBookThoughtsPanel m_Owner = null;

        private static List<ClueBookCharacterButton> s_Instances = new List<ClueBookCharacterButton>(0);

        private static CharacterData s_Selected = null;
        
        private void OnEnable()
        {
            s_Instances.Add(this);
        }

        private void OnDisable()
        {
            s_Instances.Remove(this);
        }

        public void FeedData(CharacterData characterData, ClueBookThoughtsPanel owner)
        {
            m_Character = characterData;

            m_Owner = owner;
            
            DisableHighlight();

            m_Text.text = m_Character.GetName();
        }

        private void EnableHighlight()
        {
            m_Background.color = GameManager.Instance.DevSettings.HighlightedColor;
            
            m_Extension.gameObject.SetActive(true);
            m_Extension.color = GameManager.Instance.DevSettings.HighlightedColor;
        }

        private void DisableHighlight()
        {
            m_Background.color = GameManager.Instance.DevSettings.DisabledColor;
            
            m_Extension.gameObject.SetActive(false);
        }

        public void SelectCharacter()
        {
            s_Instances.ForEach(i => i.DisableHighlight());
            
            m_Owner.ShowForCharacter(m_Character);

            s_Selected = m_Character;
            
            EnableHighlight();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (s_Selected == m_Character)
            {
                return;
            }

            EnableHighlight();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (s_Selected == m_Character)
            {
                return;
            }
            
            DisableHighlight();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SelectCharacter();
        }
    }
}