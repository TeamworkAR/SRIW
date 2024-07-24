using System.Collections.Generic;
using Core;
using UnityEngine;

namespace UI.ClueCollection.ClueBook
{
    public class ClueBookNotificationAnimation : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_CavasGroup = null;
        
        private List<Coroutine> m_Running = new List<Coroutine>(0);

        private void OnDisable()
        {
            m_Running.Clear();
            StopAllCoroutines();

            m_CavasGroup.alpha = 0f;
        }

        public void TryStartAnimation()
        {
            if (m_Running.Count > 0)
            {
                return;
            }

            m_Running.Add(StartCoroutine(Helpers.UI.COR_Scale(this.transform, Vector3.one, Vector3.one * 1.5f, 0.5f)));
            m_Running.Add(StartCoroutine(Helpers.UI.COR_Fade(m_CavasGroup, 1f, 0f, 1f, () => { m_Running.Clear(); })));
        }
    }
}