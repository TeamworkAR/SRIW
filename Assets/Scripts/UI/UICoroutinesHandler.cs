using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace UI
{
    public class UICoroutinesHandler : SingletonBehaviour<UICoroutinesHandler>
    {
        private Dictionary<object, Coroutine> m_AliveObjects = new Dictionary<object, Coroutine>(0);

        public bool IsAlive(object o) => m_AliveObjects.ContainsKey(o);

        public void TryStartCoroutine(object owner, IEnumerator coroutine) 
        {
            if (m_AliveObjects.ContainsKey(owner)) 
            {
                return;
            }

            m_AliveObjects.Add(owner, StartCoroutine(COR_Consume(owner, coroutine)));
        }

        public void TryStopCoroutine(object owner) 
        {
            if(m_AliveObjects.ContainsKey(owner) == false) 
            {
                return;
            }

            StopCoroutine(m_AliveObjects[owner]);

            m_AliveObjects.Remove(owner);
        }

        private IEnumerator COR_Consume(object owner, IEnumerator coroutine) 
        {
            yield return coroutine;

            m_AliveObjects.Remove(owner);
        }
    }
}