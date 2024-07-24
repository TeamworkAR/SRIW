using DialogueSystem;
using UnityEngine;

namespace Data.EnvironmentData
{
    [CreateAssetMenu(menuName = "Editor/" + nameof(EnvironmentData))]
    public class EnvironmentData : ScriptableObject
    {
        [SerializeField] private Environment m_Template = null;

        public Environment Template => m_Template;
    }
}