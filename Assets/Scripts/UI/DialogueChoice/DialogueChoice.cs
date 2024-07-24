using System;
using Managers;
using UnityEngine;
using UnityEngine.Localization;

namespace UI.DialogueChoice
{
    [Serializable]
    public class DialogueChoice
    {
        [SerializeField] private LocalizedString m_Text = null;

        public string GetText() => LocalizationManager.Instance.GetLocalizedValue(m_Text);
    }
}