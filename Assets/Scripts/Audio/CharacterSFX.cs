using Managers;
using UnityEngine;

namespace Audio
{
    public class CharacterSFX : MonoBehaviour
    {
        /// <summary>
        /// Used mainly by AnimationEvents
        /// </summary>
        /// <param name="clip">The clip that will be played</param>
        public void OnSFXRequestEvent(AudioClip clip)
        {
            AudioManager.Instance.DoSfx(clip);
        }
    }
}