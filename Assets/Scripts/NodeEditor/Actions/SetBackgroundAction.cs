using NodeEditor.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;


namespace NodeEditor.Actions
{
    [Serializable]
    public class SetBackgroundAction : Action
    {
        [SerializeField] private Texture m_BackgroundTexture = null;
        [SerializeField] bool show = false;

        public override void Execute()
        {
            if (m_BackgroundTexture != null)
                MainGUI.Instance.MBackgroundUI.m_BackgroundImage.texture = m_BackgroundTexture;

            if (show)
                MainGUI.Instance.MBackgroundUI.Show();
            else
                MainGUI.Instance.MBackgroundUI.Hide();

        }

    }

}
