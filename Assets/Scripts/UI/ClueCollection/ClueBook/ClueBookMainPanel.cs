using System;
using System.Collections.Generic;
using Animation;
using Data.ScenarioSettings;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ClueCollection.ClueBook
{ 
    public class ClueBookMainPanel : ClueBookUI.ClueBookPanel
    {
        [SerializeField] private List<ThoughtImage> m_ThoughtsImages = new List<ThoughtImage>(0);

        [SerializeField] private PolicyImage m_PoliciesImage = null;

        [SerializeField] private Button m_OpenThoughtsButton = null;
        
        [SerializeField] private Button m_OpenPoliciesButton = null;
        
        private ScenarioSettings.ClueCollectionExtension m_Extension = null;
        
        public override void Show()
        {
            base.Show();

            m_Extension = GameManager.Instance.ScenarioSettings
                .GetExtension<ScenarioSettings.ClueCollectionExtension>();

            int i = 0;
            
            foreach (var leftGroupCharacter in m_Extension.LeftGroupCharacters)
            {
                if (m_Extension.IsAnyThoughtUnlockedFor(leftGroupCharacter) && i < m_ThoughtsImages.Count)
                {
                    CharacterShowcase characterShowcase = leftGroupCharacter.ShowcaseTemplate
                        .GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);

                    characterShowcase.GetComponent<ThoughtCollectionAnimations>().HandleThoughtCollection();

                    m_ThoughtsImages[i].Unlock(characterShowcase.ImageTexture);

                    i++;
                }
            }
            
            foreach (var rightGroupCharacter in m_Extension.RightGroupCharacters)
            {
                if (m_Extension.IsAnyThoughtUnlockedFor(rightGroupCharacter) && i < m_ThoughtsImages.Count)
                {
                    CharacterShowcase characterShowcase = rightGroupCharacter.ShowcaseTemplate.GetInstance(this, CharacterShowcase.CameraPositions.CloseUp);

                    characterShowcase.GetComponent<ThoughtCollectionAnimations>().HandleThoughtCollection();

                    m_ThoughtsImages[i].Unlock(characterShowcase.ImageTexture);

                    i++;
                }
            }

            // This means that some character have at least one thought unlocked
            m_OpenThoughtsButton.interactable = i > 0;

            if (m_Extension.IsAnyPolicyUnlocked())
            {
                m_OpenPoliciesButton.interactable = true;
                m_PoliciesImage.Unlock(m_Extension.m_MainPolicyImage.texture);
            }
            else
            {
                m_OpenPoliciesButton.interactable = false;
                m_PoliciesImage.Lock();
            }
        }

        public override void Hide()
        {
            base.Hide();
            
            CharacterShowcase.ClearByOwner(this);
            
            foreach (var thoughtsImage in m_ThoughtsImages)
            {
                thoughtsImage.Lock();
            }

            m_PoliciesImage.Lock();
        }

        [Serializable]
        public class ThoughtImage
        {
            [SerializeField] private RawImage m_CharacterImage = null;
            
            [SerializeField] private Image m_LockedImage = null;

            public void Lock()
            {
                m_CharacterImage.texture = null;

                m_CharacterImage.enabled = false;

                m_LockedImage.enabled = true;
            }

            public void Unlock(Texture texture)
            {
                m_CharacterImage.texture = texture;

                m_CharacterImage.enabled = true;

                m_LockedImage.enabled = false;
            }
        }

        [Serializable]
        public class PolicyImage
        {
            [SerializeField] private RawImage m_PolicyImage = null;

            [SerializeField] private List<Image> m_LockedImages = null;

            public void Lock()
            {
                m_PolicyImage.texture = null;

                m_PolicyImage.enabled = false;

                foreach(Image i in m_LockedImages)
                {
                    i.enabled = true;
                }
            }

            public void Unlock(Texture texture)
            {
                m_PolicyImage.texture = texture;

                m_PolicyImage.enabled = true;

                foreach(Image i in m_LockedImages)
                {
                    i.enabled = false;
                }
            }
        }
    }
}