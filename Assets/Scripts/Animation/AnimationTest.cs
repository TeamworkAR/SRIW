using UnityEngine;

namespace Animation
{
    public class AnimationTest : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<UnityEngine.Animation>().Play();
        }

        private void Update()
        {
            Debug.Log(GetComponent<UnityEngine.Animation>().isPlaying);
        }
    }
}