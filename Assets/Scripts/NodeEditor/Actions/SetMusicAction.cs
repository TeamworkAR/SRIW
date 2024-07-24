using System;
using UnityEngine;
using Managers;

namespace NodeEditor.Actions
{
    [Serializable]
    public class SetMusicAction : Action
    {
		[SerializeField] private AudioClip m_AudioClip = null;

        [SerializeField] private bool b_IsImmediate = false;
        
        public override void Execute()
        {
            AudioManager.Instance.PlayMusic(m_AudioClip, b_IsImmediate);
        }
    }
}