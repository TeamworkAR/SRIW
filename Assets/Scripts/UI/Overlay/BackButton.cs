using System;
using Core;
using Managers;
using NodeEditor.Graphs;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Overlay
{
    public class BackButton : MonoBehaviour
    {
        [SerializeField] private Canvas m_Canvas = null;
        
        [SerializeField] private Button m_Button = null;

        private Coroutine m_Running = null;
        
        private void OnEnable()
        {
            Graph.GraphInstance.OnNodeUpdated += OnNodeUpdated;
        }

        private void OnDisable()
        {
            Graph.GraphInstance.OnNodeUpdated -= OnNodeUpdated;
        }

        private void OnNodeUpdated()
        {
            m_Canvas.enabled = GameManager.Instance.GraphInstance.GetCurrentSteps() > 1;
            
            this.GetComponentInChildren<AccessibleButton>().enabled = GameManager.Instance.GraphInstance.GetCurrentSteps() > 1;
        }

        public void Back()
        {
            //Return timescale to normal using the back button
            ApplicationPauseManager.Instance?.Resume();
            
            GameManager.Instance.Back();
            
            m_Running = StartCoroutine(Helpers.UI.COR_Cooldown(Consts.UI.k_BACK_BUTTON_CD,
                () => { m_Button.interactable = false;}, () => { m_Button.interactable = true;
                    m_Running = null;
                }));
        }
    }
}