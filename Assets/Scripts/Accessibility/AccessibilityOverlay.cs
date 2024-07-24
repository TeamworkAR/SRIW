using Managers;
using UnityEngine;
using UnityEngine.Localization;

public class AccessibilityOverlay : MonoBehaviour
{
    private Coroutine fadeInCoroutine;

    public void Show(LocalizedString localizedString, bool selectOnly = false)
    {
        SetAccessibilityText(localizedString, selectOnly);
        this.gameObject.SetActive(true);
        
        var cg = this.GetComponent<CanvasGroup>();
        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;
        //No fade in as this is just the pause button so it should match the overlay
        //fadeInCoroutine = StartCoroutine(FadeIn());
    }

    // IEnumerator FadeIn()
    // {
    //     yield return null;
    //     var cg = this.GetComponent<CanvasGroup>();
    //     cg.interactable = true;
    //     cg.blocksRaycasts = true;
    //     yield return Helpers.UI.COR_Fade(cg, 0f,
    //         1f, 1f);
    // }
    
    public void Hide()
    {
        this.gameObject.SetActive(false);
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
        }
        var cg = this.GetComponent<CanvasGroup>();
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
    
    public void SetAccessibilityText(LocalizedString localizedString, bool selectOnly = false)
    {
        var accessibleLabel = this.GetComponentInChildren<AccessibleLabel>();
        accessibleLabel.m_Text = LocalizationManager.Instance.GetLocalizedValue(localizedString);
        accessibleLabel.Select(selectOnly);
    }
}
