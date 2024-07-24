using System;
using System.Collections.Generic;
using CareBoo.Serially;
using UnityEngine;

namespace Data.ScenarioSettings
{
    [CreateAssetMenu(menuName = "Editor/ScenarionSettings")]
    public partial class ScenarioSettings : ScriptableObject
    {
        [SerializeReference, ShowSerializeReference]
        private List<ScenarioExtension> m_ScenarionExtension = new List<ScenarioExtension>(0);
        
        public T GetExtension<T>()
        where T : ScenarioExtension
        {
            foreach (var scenarioExtension in m_ScenarionExtension)
            {
                if (scenarioExtension.GetType() == typeof(T))
                {
                    return (T)scenarioExtension;
                }
            }

            // TODO: Handle error.
            return null;
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            m_ScenarionExtension.ForEach(e => e.OnValidate());
        }
#endif
        
        [Serializable]
        public abstract class ScenarioExtension
        {
#if UNITY_EDITOR
            public virtual void OnValidate()
            {
            }
#endif   
        }
    }
}