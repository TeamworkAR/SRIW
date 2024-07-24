using System;
using Data.ScenarioSettings;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class LockPortableThoughtsAction : Action
    {
        [SerializeField] private PortableThoughtsWrapper m_PortableThoughtsWrapper = null;
        
        public override void Execute()
        {
            ScenarioSettings.ClueCollectionExtension.Lock(m_PortableThoughtsWrapper);
        }
    }
}