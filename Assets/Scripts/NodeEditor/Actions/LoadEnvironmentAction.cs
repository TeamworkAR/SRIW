using System;
using Core;
using Managers;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class LoadEnvironmentAction : Action
    {
        [SerializeField] private SceneReference m_Scene = null;
        
        public override void Execute()
        {
            EnvironmentManager.Instance.LoadEnvironment(m_Scene);
        }
    }
}