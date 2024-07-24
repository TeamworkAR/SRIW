using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScenarioSettings
{
    [CreateAssetMenu(menuName = "Editor/" + nameof(SequencialInfoCardDataWrapper), fileName = "new" + nameof(SequencialInfoCardDataWrapper))]
    public class SequencialInfoCardDataWrapper : ScriptableObject
    {
        [SerializeField] private SequencialInfoCardData sequencialInfoCardData = null;

        public SequencialInfoCardData SequencialInfoCardData => sequencialInfoCardData;

    }
}
