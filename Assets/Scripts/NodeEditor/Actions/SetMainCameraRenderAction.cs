using System;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable] 
    public class SetMainCameraRenderAction : Action
    {
        [SerializeField] private LayerMask m_mask;

        public override void Execute()
        {
            // Disabled this action, only DialogueManager will carry on camera operations.
            return;
            Camera.main.cullingMask = m_mask;
        }
    }

}

