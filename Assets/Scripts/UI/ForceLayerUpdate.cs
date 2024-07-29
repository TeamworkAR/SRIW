using UnityEngine;
using UnityEngine.UI;

public class ForceLayoutUpdate : MonoBehaviour
{
    public RectTransform layoutGroup;
    public Canvas canvas;

    void Start()
    {
        canvas.enabled = true;
        layoutGroup.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup);
    }
}
