using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackValidator : MonoBehaviour
{
    [SerializeField] private Button submitButton;

    [SerializeField] private TMPro.TMP_InputField summaryField;
    [SerializeField] private TMPro.TMP_InputField emailField;
    [SerializeField] private TMPro.TMP_InputField descriptionField;

    // Start is called before the first frame update
    void Start()
    {
        submitButton.interactable = false;
        summaryField.onValueChanged.AddListener(ValidationHandler);
        emailField.onValueChanged.AddListener(ValidationHandler);
        descriptionField.onValueChanged.AddListener(ValidationHandler);
    }

    private void OnDestroy()
    {
        summaryField.onValueChanged.RemoveListener(ValidationHandler);
        emailField.onValueChanged.RemoveListener(ValidationHandler);
        descriptionField.onValueChanged.RemoveListener(ValidationHandler);
    }

    private void ValidationHandler(string arg)
    {
        Validate();
    }

    private void Validate()
    {
        var hasSummary = !string.IsNullOrEmpty(summaryField.text);
        var isValidEmail = IsValidEmail(emailField.text);
        var hasDescription = !string.IsNullOrEmpty(descriptionField.text);
        
        submitButton.interactable = hasSummary && isValidEmail && hasDescription;
    }
    
    //validation from: https://learn.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException e)
        {
            return false;
        }
        catch (ArgumentException e)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
