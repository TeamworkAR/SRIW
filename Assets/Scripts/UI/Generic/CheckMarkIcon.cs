using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Generic
{
    public class CheckMarkIcon : MonoBehaviour
    {
        [SerializeField] private Image m_Background = null;

        private void Start()
        {
            m_Background.color = GameManager.Instance.DevSettings.UIGreen;
        }
    }
}
