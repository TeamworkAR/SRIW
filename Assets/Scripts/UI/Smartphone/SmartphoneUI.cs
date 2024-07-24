using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Smartphone
{
    public class SmartphoneUI : BaseUICanvas
    {
        [SerializeField] private RectTransform m_EntriesContainer = null;
        
        private SmartphoneChain m_Chain = null;
        
        private Coroutine m_Running = null;

        public RectTransform EntriesContainer => m_EntriesContainer;
        
        public void Show(SmartphoneChain chain)
        {
            this.GetComponent<AccessibleUIGroupRoot>().enabled = true;
            m_Chain = chain;
            
            base.Show();
            
            TryStartLifeCycle();
        }

        public override void Show()
        {
            Debug.LogError(
                $"Calling {nameof(BaseUICanvas)}.Show from {nameof(SmartphoneUI)}, call its overload instead");
        }

        public override void Hide()
        {
            base.Hide();
            
            TryStopLifeCycle();
            
            foreach (Transform t in m_EntriesContainer)
            {
                Destroy(t.gameObject);
            }
            
            m_Chain = null;
            this.GetComponent<AccessibleUIGroupRoot>().enabled = false;
        }

        private void TryStartLifeCycle()
        {
            if (m_Running == null)
            {
                m_Running = StartCoroutine(COR_LifeCycle());
            }
        }

        private void TryStopLifeCycle()
        {
            if (m_Running != null)
            {
                StopCoroutine(m_Running);
                m_Running = null;
            }
        }

        private IEnumerator COR_LifeCycle()
        {
            foreach (var smartphoneChainEntry in m_Chain.Entries)
            {
                smartphoneChainEntry.Create(this);

                smartphoneChainEntry.DoSfx();
                
                float t = 0f;

                while (t <= GameManager.Instance.DevSettings.MessagesWaitTime)
                {
                    t += Time.deltaTime;

                    LayoutRebuilder.ForceRebuildLayoutImmediate(m_EntriesContainer);

                    yield return new WaitForSeconds(0.00001f);
                }
            }
            
            Hide();
        }
    }
}