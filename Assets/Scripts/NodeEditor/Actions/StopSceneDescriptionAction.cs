using System;
using Data.DialogueData;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StopSceneDescriptionAction : Action
    {
        public override void Execute() => MainGUI.Instance.SceneDescription.Hide();
    }
}