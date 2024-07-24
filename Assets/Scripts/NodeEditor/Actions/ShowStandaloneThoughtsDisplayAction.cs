using System;
using Data.ScenarioSettings;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public sealed class ShowStandaloneThoughtsDisplayAction : Action
    {
        [SerializeField] private PortableThoughtsWrapper m_PortableThoughtsWrapper = null;
        
        public override void Execute()
        {
            MainGUI.Instance.StandaloneThoughtsDisplay.ShowStandalone(m_PortableThoughtsWrapper);
        }
    }
}