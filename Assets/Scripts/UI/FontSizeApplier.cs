using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

public class FontSizeApplier : MonoBehaviour
{
    public FontSizeSettings fontSizeSettings;
    public TextMeshProUGUI uiText;

    void OnEnable()
    {
        ApplyFontSize();
    }

    public void ApplyFontSize()
    {
        var currentLocale = LocalizationSettings.SelectedLocale;
        string languageCode = currentLocale.Identifier.Code;

        int fontSize = fontSizeSettings.GetFontSize(languageCode);

        // Disable auto size if enabled
        if (uiText.enableAutoSizing)
        {
            uiText.enableAutoSizing = false;
        }

        // Apply the new font size
        uiText.fontSize = fontSize;
    }
}
