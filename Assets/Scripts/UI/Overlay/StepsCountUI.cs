using System.Collections;
using System.Collections.Generic;
using Managers;
using NodeEditor.Graphs;
using UnityEngine;

namespace UI.Overlay
{
    public class StepsCountUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_CanvasGroup = null;
        
        [SerializeField] private RectTransform m_StepsVizContainer = null;

        [SerializeField] private StepViz m_StepVizTemplate = null;

        private List<StepViz> m_StepsVizInstances = new List<StepViz>(0);
        
        private void Start()
        {
            Graph.GraphInstance.OnNodeUpdated += OnNodeUpdated;
        }

        private void OnDisable()
        {
            Graph.GraphInstance.OnNodeUpdated -= OnNodeUpdated;
        }

        public void Show()
        {
            m_CanvasGroup.alpha = 1f;
        }

        public void Hide()
        {
            m_CanvasGroup.alpha = .1f;
        }

        private void OnNodeUpdated()
        {
            // TODO: Implement some sort of pooling
            foreach (var stepsVizInstance in m_StepsVizInstances)
            {
                Destroy(stepsVizInstance.gameObject);    
            }
            m_StepsVizInstances.Clear();

            for (int i = 0; i < GameManager.Instance.GraphInstance.GetCurrentSteps(); i++)
            {
                StepViz viz = Instantiate(m_StepVizTemplate, m_StepsVizContainer);

                if (i == GameManager.Instance.GraphInstance.GetCurrentSteps() - 1)
                {
                    viz.SetActive();
                }
                else
                {
                    viz.SetCompleted();
                }
                
                m_StepsVizInstances.Add(viz);
            }
            
            for (int i = 0; i < GameManager.Instance.GraphInstance.GetMissingSteps(); i++)
            {
                StepViz viz = Instantiate(m_StepVizTemplate, m_StepsVizContainer);

                viz.SetMissing();
                
                m_StepsVizInstances.Add(viz);
            }
        }
    }
}