using UnityEngine;

namespace Data.ScenarioSettings
{
    [CreateAssetMenu(menuName = "Editor/" + nameof(BasicInfoPanelDataWrapper), fileName = "new" + nameof(BasicInfoPanelDataWrapper))]
    public class BasicInfoPanelDataWrapper : ScriptableObject
    {
        [SerializeField] private BasicInfoPanelData basicInfoPanelData = null;

        public BasicInfoPanelData BasicInfoPanelData => basicInfoPanelData;

    }
}

