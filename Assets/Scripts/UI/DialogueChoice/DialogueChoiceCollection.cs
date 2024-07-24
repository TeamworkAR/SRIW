using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Localization;

namespace UI.DialogueChoice
{
    [Serializable]
    public class DialogueChoiceCollection
    {
        [SerializeField] private LocalizedString m_Title = null;

        [SerializeField] private List<DialogueChoice> m_Choices = new List<DialogueChoice>(0);

        public List<DialogueChoice> Choices => m_Choices;

        public string GetTitle() => LocalizationManager.Instance.GetLocalizedValue(m_Title);
    }
}