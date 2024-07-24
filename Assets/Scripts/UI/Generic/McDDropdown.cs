using System.Collections.Generic;
using Core;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Generic
{
    public class McDDropdown : TMP_Dropdown
    {
        [SerializeField] private List<Graphic> m_TargetGraphics = new List<Graphic>(0);

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (GameManager.Instance == null)
            {
                return;
            }

            if (!gameObject.activeInHierarchy)
                return;

            Color tintColor;

            switch (state)
            {
                case SelectionState.Normal:
                    tintColor = GameManager.Instance.DevSettings.NormalColor;
                    break;
                case SelectionState.Highlighted:
                    tintColor = GameManager.Instance.DevSettings.HighlightedColor;
                    break;
                case SelectionState.Pressed:
                    tintColor = GameManager.Instance.DevSettings.PressedColor;
                    break;
                case SelectionState.Selected:
                    tintColor = GameManager.Instance.DevSettings.SelectedColor;
                    break;
                case SelectionState.Disabled:
                    tintColor = GameManager.Instance.DevSettings.DisabledColor;
                    break;
                default:
                    tintColor = Color.black;
                    break;
            }

            foreach (var graphic in m_TargetGraphics)
            {
                graphic.CrossFadeColor(tintColor, Consts.UI.k_BUTTON_FADE_DURATION, true, true);
            }
        }    
    }
}