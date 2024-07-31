using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class HangingIndent : MonoBehaviour
{
    public float indentAmount = 20f; // Amount to indent in percentage
    [SerializeField]private TextMeshProUGUI tmpText;

    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        ApplyHangingIndent();
    }

    void ApplyHangingIndent()
    {
        string text = tmpText.text;
        string[] lines = text.Split('\n');
        string result = "";
        bool isBulletPoint = false;

        foreach (string line in lines)
        {
            if (line.TrimStart().StartsWith("•"))
            {
                if (isBulletPoint)
                {
                    result += "</indent>\n";
                }
                isBulletPoint = true;
                result += line + "\n<indent=" + indentAmount + "%>";
            }
            else if (isBulletPoint)
            {
                result += line + "\n";
            }
            else
            {
                result += line + "\n";
            }
        }

        if (isBulletPoint)
        {
            result += "</indent>";
        }

        tmpText.text = result;
    }
}
