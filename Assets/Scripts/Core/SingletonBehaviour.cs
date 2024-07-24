using System;
using UnityEngine;

namespace Core
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour
    where T : SingletonBehaviour<T>
    {
        private static T m_Instance = null;

        public static T Instance => m_Instance;

        protected virtual void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this as T;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void OnDestroy()
        {
            if (m_Instance == this)
            {
                Destroy(this);
            }
        }
    }
}