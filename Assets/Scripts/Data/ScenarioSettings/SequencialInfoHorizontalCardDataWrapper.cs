using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScenarioSettings
{
    [CreateAssetMenu(menuName = "Editor/" + nameof(SequencialInfoHorizontalCardDataWrapper), fileName = "new" + nameof(SequencialInfoHorizontalCardDataWrapper))]
    public class SequencialInfoHorizontalCardDataWrapper : ScriptableObject
    {
        [SerializeField] private SequencialInfoHorizontalCardData sequencialInfoCardData = null;

        public SequencialInfoHorizontalCardData SequencialInfoCardData => sequencialInfoCardData;

    }
}
