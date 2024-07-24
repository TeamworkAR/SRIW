using System;
using System.Collections;
using System.Runtime.InteropServices;
using Managers;
using UnityEngine;
using UnityEngine.Localization;

public class MobileBrowserDetection : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern bool IsMobile();
    [DllImport("__Internal")]
    private static extern void SetAccessibilityText(string text);
    public static bool IsMobileBrowser { get; private set; }

    [SerializeField]
    private LocalizedString accessibilityText;
    
    void Start()
    {
        DetectMobile();
        StartCoroutine(PopulateAccessibilityButton());
    }

    public bool DetectMobile()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
         IsMobileBrowser = IsMobile();
#endif
        return IsMobileBrowser;
    }

    IEnumerator PopulateAccessibilityButton()
    {
        while (!LocalizationManager.Instance.Initialized)
        {
            yield return null;
        }
#if !UNITY_EDITOR && UNITY_WEBGL
        SetAccessibilityText(LocalizationManager.Instance.GetLocalizedValue(accessibilityText));
#endif
        Debug.Log("Accessibility Text:"+LocalizationManager.Instance.GetLocalizedValue(accessibilityText));
    }
}