using Managers;
using System.Collections;
using System.Collections.Generic;
using UI.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequencialAnswerButton : MonoBehaviour
{
    [SerializeField] private SequencialInfoHorizontalCard card;
    [SerializeField] private Image border;
    [SerializeField] private GameObject crossTick;
    [SerializeField] private Color targetColor;
    [SerializeField] private GameObject message;
    [SerializeField] private Color startColor;

    private McDButton button;

    private void Start()
    {
        button = GetComponent<McDButton>();
        button.onClick.AddListener(() => ChangeColor(targetColor));
        startColor = border.color;
        message.SetActive(false);
    }

    public void ChangeColor(Color _color)
    {
        card.cardController.SelectedAwnser();
        card.ActivateHiddenWheel();
        if (!card.cardData.isCorrectAwnser)
        {
            border.color = _color;
            crossTick.SetActive(true);
            message.SetActive(true);
        }
        else
        {
            card.cardController.CorrectAwsnerSelected();
        }
        
    }

    public void ResetColor()
    {
        border.color = startColor;
        crossTick.SetActive(false);
        message.SetActive(false);
    }
}


