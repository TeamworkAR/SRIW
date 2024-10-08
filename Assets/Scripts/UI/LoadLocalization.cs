using System.Collections;
using Core;
using Managers;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class LoadLocalization : MonoBehaviour
{
    [SerializeField]
    private string sceneToUnload = "LoadingScene";
    [SerializeField]
    private string sceneOnLoad = "McScene";
    [SerializeField]
    private CanvasGroup canvas;
    [SerializeField]
    private float fadeTime = 0.25f;
    private void Start()
    {
        StartCoroutine(WaitForLocalization());
    }
    IEnumerator WaitForLocalization()
    {   
        // Wait for localization to initialize
        // This is useful (and mandatory) when restoring a node, else our localized UIs might broke.
        yield return LocalizationSettings.InitializationOperation;

        // Try restore previously selected locale
        var suspendedData = ScormManager.Instance.GetCustomString(Consts.ScormKeys.k_LOCALIZATION_SERIALIZATION_KEY);
        if (string.IsNullOrEmpty(suspendedData) || LocalizationSettings.AvailableLocales.Locales.Count <= int.Parse(suspendedData))
        {

        }
        else 
        {
            var newLocale = LocalizationSettings.AvailableLocales.Locales[int.Parse(suspendedData)];
            if (newLocale != LocalizationSettings.SelectedLocale)
            {
                LocalizationManager.Instance.InvalidateLocale();
                LocalizationSettings.SelectedLocale = newLocale;
            }
        }

        while (!LocalizationManager.Instance.IsLoaded)
        {
            yield return null;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneOnLoad, LoadSceneMode.Additive);
    }
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StartCoroutine(FadeCanvas());
    }

    IEnumerator FadeCanvas()
    {
        var fadeRemaining = fadeTime;
        while (fadeRemaining > 0f)
        {
            canvas.alpha = (fadeRemaining / fadeTime);
            yield return null;
            fadeRemaining -= Time.deltaTime;
        }
        SceneManager.UnloadSceneAsync(sceneToUnload);
    }
}
