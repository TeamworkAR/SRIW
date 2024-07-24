using System;
using Data.DialogueData;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartSceneDescriptionAction : Action
    {
        [SerializeField] private DialogueData m_Dialogue = null;
        [SerializeField] private bool m_selectOnly = false;

        public override void Execute() => MainGUI.Instance.SceneDescription.Show(m_Dialogue.AccessibilityDescription, m_selectOnly);
    }
}