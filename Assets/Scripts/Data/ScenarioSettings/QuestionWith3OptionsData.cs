using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Data.ScenarioSettings
{
    [System.Serializable]
    public class QuestionWith3OptionsData
    {
        [SerializeField] private CharacterData.CharacterData owner = null;
        [SerializeField] private LocalizedString questionText;
        [SerializeField] private LocalizedString correctResponseTitle;
        [SerializeField] private LocalizedString correctResponseText;
        [SerializeField] private int correctResponseIndex;
        [SerializeField] private List<bool> buttonStates = new List<bool> { true, true, true }; // Default to all buttons on

        public CharacterData.CharacterData Owner => owner;
        public LocalizedString QuestionText => questionText;
        public LocalizedString CorrectResponseTitle => correctResponseTitle;
        public LocalizedString CorrectResponseText => correctResponseText;
        public int CorrectResponseIndex => correctResponseIndex;
        public List<bool> ButtonStates => buttonStates;
    }
}
