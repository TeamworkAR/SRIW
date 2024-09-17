using UnityEngine;

namespace Data.ScenarioSettings
{
    [CreateAssetMenu(menuName = "Editor/" + nameof(DisclaimerPanelDataWrapper), fileName = "new" + nameof(DisclaimerPanelDataWrapper))]
    public class DisclaimerPanelDataWrapper : ScriptableObject
    {
        [SerializeField] private DisclaimerPanelData disclaimerPanelData = null;

        public DisclaimerPanelData DisclaimerPanelData => disclaimerPanelData;

    }
}

