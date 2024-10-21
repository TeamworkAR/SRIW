using System;
using System.Collections.Generic;
using CareBoo.Serially;
using Core;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Generic
{
    public class McDButton : Button
    {
        [SerializeField] private List<Graphic> m_TargetGraphics = new List<Graphic>(0);

        [SerializeReference, ShowSerializeReference] private ButtonStatesColorOverride m_Override = null;

        protected override void Awake()
        {
            base.Awake();

            if (m_Override == null)
            {
                m_Override = new DefaultButtonStateColorOverride();
            }
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (GameManager.Instance == null || !gameObject.activeInHierarchy)
                return;

            Color tintColor = m_Override.GetColor(state);

            foreach (var graphic in m_TargetGraphics)
            {
                // Ensure we don't fade text objects unintentionally
                if (graphic is TextMeshProUGUI)
                    continue;  // Skip fading for TextMeshPro components

                try
                {
                    graphic.CrossFadeColor(tintColor, Consts.UI.k_BUTTON_FADE_DURATION, true, true);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }


        [Serializable]
        protected abstract class ButtonStatesColorOverride
        {
            public abstract Color GetColor(SelectionState state);
        }

        [Serializable]
        protected class DefaultButtonStateColorOverride : ButtonStatesColorOverride
        {
            public override Color GetColor(SelectionState state)
            {
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
                        tintColor = Color.magenta;
                        break;
                }

                return tintColor;
            }
        }

        [Serializable]
        protected class ChevronsButtonStateColorOverride : ButtonStatesColorOverride
        {
            public override Color GetColor(SelectionState state)
            {
                Color tintColor;
                
                switch (state)
                {
                    case SelectionState.Normal:
                    case SelectionState.Highlighted:
                    case SelectionState.Pressed:
                    case SelectionState.Selected:
                        tintColor = GameManager.Instance.DevSettings.ChevronsNormalColor;
                        break;
                    case SelectionState.Disabled:
                        tintColor = GameManager.Instance.DevSettings.ChevronsDisabledColor;
                        break;
                    default:
                        tintColor = Color.magenta;
                        break;
                }

                return tintColor;
            }
        }

        [Serializable]
        protected class LearningsButtonStateColorOverride : ButtonStatesColorOverride
        {
            public override Color GetColor(SelectionState state)
            {
                Color tintColor;

                switch (state)
                {
                    case SelectionState.Normal:
                        tintColor = GameManager.Instance.DevSettings.FoundationalGold;
                        break;
                    case SelectionState.Highlighted:
                        tintColor = GameManager.Instance.DevSettings.ChevronsNormalColor;
                        break;
                    case SelectionState.Pressed:
                        tintColor = GameManager.Instance.DevSettings.ChevronsNormalColor;
                        break;
                    case SelectionState.Selected:
                        tintColor = GameManager.Instance.DevSettings.ChevronsNormalColor;
                        break;
                    case SelectionState.Disabled:
                        tintColor = GameManager.Instance.DevSettings.DisabledColor;
                        break;
                    default:
                        tintColor = Color.magenta;
                        break;
                }

                return tintColor;
            }
        }
    }
}