using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Generic
{
    public class CarouselCounterContainer : MonoBehaviour
    {
        [SerializeField] private CarouselCounter m_CarouselCounterTemplate = null;

        [SerializeField] private GameObject m_LeftButton = null;

        [SerializeField] private GameObject m_RightButton = null;
        
        private List<CarouselCounter> m_CarouselCounters = new List<CarouselCounter>(0);

        private bool b_Cycle = false;

        public void Init(int count, bool cycle = false)
        {
            b_Cycle = cycle;

            for (int i = 0; i < count; i++)
            {
                CarouselCounter counter = Instantiate(m_CarouselCounterTemplate, transform);
                counter.UnselectedViz();
                m_CarouselCounters.Add(counter);
            }
            m_CarouselCounters[0].SelectedViz();
        }

        public void Dispose()
        {
            foreach (var carouselCounter in m_CarouselCounters)
            {
                Destroy(carouselCounter.gameObject);
            }
            m_CarouselCounters.Clear();
        }

        public void Select(int selected)
        {
            for (int i = 0; i < m_CarouselCounters.Count; i++)
            {
                if (i == selected)
                {
                    m_CarouselCounters[i].SelectedViz();
                }
                else
                {
                    m_CarouselCounters[i].UnselectedViz();
                }
            }
            
            EnableInteraction(selected);
        }

        public void DisableInteraction()
        {
            m_LeftButton.GetComponentInChildren<Button>().interactable = false;
            m_RightButton.GetComponentInChildren<Button>().interactable = false;
        }

        public void EnableInteraction(int selected)
        {
            m_LeftButton.GetComponentInChildren<Button>().interactable = b_Cycle || selected != 0;
            m_RightButton.GetComponentInChildren<Button>().interactable = b_Cycle || selected != m_CarouselCounters.Count - 1;
        }

        public void SetChevronsActiveState(bool state)
        {
            m_LeftButton.SetActive(state);
            m_RightButton.SetActive(state);
        }
    }
}