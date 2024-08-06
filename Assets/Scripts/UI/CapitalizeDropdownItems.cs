using UnityEngine;
using TMPro;
using System.Globalization;

public class CapitalizeTMPDropdownItems : MonoBehaviour
{
    public TMP_Dropdown languageDropdown;

    void Update()
    {
        CapitalizeItems();
    }

    void CapitalizeItems()
    {
        for (int i = 0; i < languageDropdown.options.Count; i++)
        {
            string originalText = languageDropdown.options[i].text;

            // Check if the item is "fran�ais" and change it to "Fran�ais (Canada)"
            if (originalText.ToLower() == "fran�ais")
            {
                originalText = "Fran�ais (Canada)";
            }

            // Capitalize each word in the text
            string capitalizedText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(originalText.ToLower());
            languageDropdown.options[i].text = capitalizedText;
        }

        // Refresh the dropdown to show the updated text
        languageDropdown.RefreshShownValue();
    }
}
