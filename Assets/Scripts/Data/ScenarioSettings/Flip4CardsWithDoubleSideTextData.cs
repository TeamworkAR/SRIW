using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Data.ScenarioSettings
{
    [System.Serializable]
    public class Flip4CardsWithDoubleSideTextData
    {
        [SerializeField] private LocalizedString messageText;
        [SerializeField] private List<DoubleTextCard> cardTexts;

        public LocalizedString MessageText => messageText;
        public List<DoubleTextCard> CardTexts => cardTexts;
    }

    [System.Serializable]
    public class DoubleTextCard
    {
        public LocalizedString Front;
        public LocalizedString Back;
    }
}