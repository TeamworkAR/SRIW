using System;
using TMPro;
using UnityEngine;

namespace UI.DialogueChoice
{
    public class DialogueChoiceDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Text = null;

        public static event Action<DialogueChoice> OnChoiceSelected = null;
        
        private DialogueChoice m_Data = null;
        
        public void FeedData(DialogueChoice data)
        {
            m_Data = data;

            m_Text.text = m_Data.GetText();
        }

        public void SelectChoice() => OnChoiceSelected?.Invoke(m_Data);
    }
}