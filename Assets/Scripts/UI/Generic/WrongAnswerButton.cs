using Managers;
using System.Collections;
using System.Collections.Generic;
using UI.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WrongAnswerButton : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int responseIndex;

    [Header("Visual settings")]
    [SerializeField] private Image border;
    [SerializeField] private Color targetColor;
    [SerializeField] private Color resetColor = Color.white;
    [SerializeField] private GameObject message;

    private bool isCorrectResponse;

    private McDButton button;

    private UnityAction onClickCallback;

    private void Start()
    {
        button = GetComponent<McDButton>();
        button.onClick.AddListener(() => OnClick(targetColor));

        message.SetActive(false);
    }

    private void OnClick(Color _color)
    {
        if (isCorrectResponse)
        {
            onClickCallback.Invoke();
        }
        else
        {
            border.color = _color;
            message.SetActive(true);
        }
    }

    public void ResetColor()
    {
        border.color = resetColor;
        message.SetActive(false);
    }

    public void ResetCorrectResponse()
    {
        isCorrectResponse = false;
    }

    public void SetAsCorrectResponse(UnityAction _onClickCallback)
    {
        onClickCallback = _onClickCallback;
        isCorrectResponse = true;
    }
}


