using AeLa.EasyFeedback;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Feedback
{
    public class FeedbackButton : MonoBehaviour
    {
        [SerializeField] private Button ToggleFormButton = null;
        [SerializeField] private FeedbackForm EasyFeedbackForm;
        [SerializeField] private Image UiBlocker;
        private void Awake()
        {
            ToggleFormButton.onClick.AddListener(ToggleFeedback);    
            EasyFeedbackForm.OnFormOpened.AddListener(UpdateBlocker);
            EasyFeedbackForm.OnFormClosed.AddListener(UpdateBlocker);
        }

        private void OnDestroy()
        {
            ToggleFormButton.onClick.RemoveListener(ToggleFeedback);
            EasyFeedbackForm.OnFormOpened.RemoveListener(UpdateBlocker);
            EasyFeedbackForm.OnFormClosed.RemoveListener(UpdateBlocker);
        }

        private void ToggleFeedback()
        {
            EasyFeedbackForm.Toggle();
        }

        private void UpdateBlocker()
        {
            UiBlocker.gameObject.SetActive(EasyFeedbackForm.IsOpen);
        }
    }
}