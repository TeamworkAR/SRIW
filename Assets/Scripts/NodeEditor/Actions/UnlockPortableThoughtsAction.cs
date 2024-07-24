using System;
using Data.ScenarioSettings;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class UnlockPortableThoughtsAction : Action
    {
        [SerializeField] private PortableThoughtsWrapper m_PortableThoughtsWrapper = null;
        
        public override void Execute()
        {
            ScenarioSettings.ClueCollectionExtension.Unlock(m_PortableThoughtsWrapper);
        }
    }
}