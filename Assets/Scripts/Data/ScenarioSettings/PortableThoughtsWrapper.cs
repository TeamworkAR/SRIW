using UnityEngine;

namespace Data.ScenarioSettings
{
    [CreateAssetMenu(menuName = "Editor/"+nameof(PortableThoughtsWrapper), fileName = "new"+nameof(PortableThoughtsWrapper))]
    public class PortableThoughtsWrapper : ScriptableObject
    {
        [SerializeField] private ScenarioSettings.ClueCollectionExtension.ThoughtsWrapper m_Wrapper = null;

        public ScenarioSettings.ClueCollectionExtension.ThoughtsWrapper Wrapper => m_Wrapper;
    }
}