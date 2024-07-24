using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Generic
{
    public class CarouselCounter : MonoBehaviour
    {
        [SerializeField] private Image m_Image = null;

        public void SelectedViz() => m_Image.color = GameManager.Instance.DevSettings.CarouselCounterSelected;

        public void UnselectedViz() => m_Image.color = GameManager.Instance.DevSettings.CarouselCounterUnselected;
    }
}