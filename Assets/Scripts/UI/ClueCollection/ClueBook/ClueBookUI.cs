using Assets.Scripts.UI.ClueCollection.ClueBook;
using System;
using UnityEngine;

namespace UI.ClueCollection.ClueBook
{
    public class ClueBookUI : PanelContainerUI<ClueBookUI.ClueBookPanel>
    {
        [SerializeField] private GameObject m_Background = null;

        [SerializeField] private ClueBookButton m_ClueBookButton = null;

        public static event Action OnClueBookShow = null;
        
        public static event Action OnClueBookHide = null;

        public static event Action OnClueBookActivated = null;

        public static event Action OnClueBookDeactivated = null;

        public override void Show()
        {
            base.Show();
            
            m_Background.SetActive(true);

            // TODO: Note for Jason, uncomment this code when reworking Notebook accesibility.
            //this.GetComponent<AccessibleUIGroupRoot>().enabled = true;

            OnClueBookShow?.Invoke();
        }

        public override void Hide()
        {
            base.Hide();
            
            m_Background.SetActive(false);

            // TODO: Note for Jason, uncomment this code when reworking Notebook accesibility.
            // this.GetComponent<AccessibleUIGroupRoot>().enabled = false;

            OnClueBookHide?.Invoke();
        }

        public void ActivateClueBook()
        {
            m_ClueBookButton.gameObject.SetActive(true);

            OnClueBookActivated?.Invoke();
        }

        public void DeactivateClueBook() 
        {
            m_ClueBookButton.gameObject.SetActive(false);

            OnClueBookDeactivated?.Invoke();
        }

        public abstract class ClueBookPanel : Panel
        {
        }   
    }
}