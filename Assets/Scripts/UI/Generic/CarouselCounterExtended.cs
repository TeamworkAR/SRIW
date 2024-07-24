using Managers;
using UI.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarouselCounterExtended : CarouselCounter
{
    [SerializeField] private Image selectedVariation;

    public void SelectedViz() => selectedVariation.gameObject.SetActive(true);

    public void UnselectedViz() => selectedVariation.gameObject.SetActive(false);
}
