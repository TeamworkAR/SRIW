using UnityEngine;

namespace DialogueSystem
{
    public class Environment : MonoBehaviour
    {
        public Environment GetInstance()
        {
            return Instantiate(this, Vector3.zero, Quaternion.identity);
        }
    }
}