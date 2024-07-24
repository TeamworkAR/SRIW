using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScenarioSettings
{
    [CreateAssetMenu(menuName = "Editor/" + nameof(CarouselPanelDataWrapper), fileName = "new" + nameof(CarouselPanelDataWrapper))]
    public class CarouselPanelDataWrapper : ScriptableObject
    {
        [SerializeField] private CarouselPanelData sequencialInfoCardData = null;

        public CarouselPanelData SequencialInfoCardData => sequencialInfoCardData;

    }
}
