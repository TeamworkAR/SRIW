using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScenarioSettings
{
    [CreateAssetMenu(menuName = "Editor/" + nameof(SingleFlipCardDataWrapper), fileName = "new" + nameof(SingleFlipCardDataWrapper))]
    public class SingleFlipCardDataWrapper : ScriptableObject
    {
        [SerializeField] private SingleFlipPanelData singleFlipCardData = null;

        public SingleFlipPanelData SingleFlipCardData => singleFlipCardData;

    }
}
