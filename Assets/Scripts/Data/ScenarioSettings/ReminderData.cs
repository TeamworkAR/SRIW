using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Data.ScenarioSettings
{
    [System.Serializable]
    public class ReminderData
    {
        [SerializeField] private LocalizedString message;
        [SerializeField] private CharacterData.CharacterData character = null;

        public LocalizedString Message => message;
        public CharacterData.CharacterData Character => character;
    }
}