using System;
using UnityEngine;
using UnityEngine.Events;

namespace NodeEditor.Actions
{
    [Serializable]
    public class UnityEventAction : Action
    {
        [SerializeField] private UnityEvent m_Event = null;

        public override void Execute()
        {
            m_Event?.Invoke();
        }
    }
}