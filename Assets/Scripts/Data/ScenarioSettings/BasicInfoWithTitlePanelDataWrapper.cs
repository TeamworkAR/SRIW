using UnityEngine;

namespace Data.ScenarioSettings
{
    [CreateAssetMenu(menuName = "Editor/" + nameof(BasicInfoWithTitlePanelDataWrapper), fileName = "new" + nameof(BasicInfoWithTitlePanelDataWrapper))]
    public class BasicInfoWithTitlePanelDataWrapper : ScriptableObject
    {
        [SerializeField] private BasicInfoWithTitlePanelData basicInfoWithTitlePanelData = null;

        public BasicInfoWithTitlePanelData BasicInfoWithTitlePanelData => basicInfoWithTitlePanelData;

    }
}

