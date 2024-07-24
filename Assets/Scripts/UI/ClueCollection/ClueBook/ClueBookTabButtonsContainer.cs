using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.ClueCollection.ClueBook
{
    public class ClueBookTabButtonsContainer : MonoBehaviour
    {
        private RectTransform m_Transform = null;

        private void Start()
        {
            m_Transform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            // Need to constantly refresh horizontal layout...
            // We are now enabling/disabling UI game objects based on the UI status
            // If This behaviour changes please remember to move this call into a Coroutine.
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_Transform);
        }
    }
}