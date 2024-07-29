using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FontSizeSettings", menuName = "Localization/FontSizeSettings")]
public class FontSizeSettings : ScriptableObject
{
    public List<LanguageFontSize> languageFontSizes;

    [System.Serializable]
    public class LanguageFontSize
    {
        public string languageCode;
        public int fontSize;
    }

    public int GetFontSize(string languageCode)
    {
        var setting = languageFontSizes.Find(x => x.languageCode == languageCode);
        return setting != null ? setting.fontSize : 14; // Default font size
    }
}
