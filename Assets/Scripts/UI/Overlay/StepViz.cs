using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Overlay
{
    public class StepViz : MonoBehaviour
    {
        [SerializeField] private Image m_Image = null;
        
        public void SetCompleted()
        {
            m_Image.color = GameManager.Instance.DevSettings.UIGreen;
        }

        public void SetActive()
        {
            m_Image.color = GameManager.Instance.DevSettings.FoundationalGold;
        }

        public void SetMissing()
        {
            m_Image.color = Color.white;
        }
    }
}