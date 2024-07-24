using UnityEngine;

namespace Audio
{
    /// <summary>
    /// Our animators are in a child of the FBXWrapper, in order to avoid to author all that's needed
    /// for our SFXs to be played for each character, we rely on this component to instantiate what we need at runtime.
    /// </summary>
    public class CharacterSFXSetup : MonoBehaviour
    {
        private void Start()
        {
            Animator animator = GetComponentsInChildren<Animator>()[1];

            if (animator == null)
            {
                return;
            }

            animator.gameObject.AddComponent<CharacterSFX>();
        }
    }
}