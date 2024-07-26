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

        [SerializeField] private List<int> stepsPerUnit = new List<int>(); // New field to set the number of steps per unit

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

            int totalSteps = GameManager.Instance.GraphInstance.GetCurrentSteps();
            int totalMissingSteps = GameManager.Instance.GraphInstance.GetMissingSteps();
            int currentStepIndex = 0;
            int currentMissingStepIndex = 0;

            foreach (int stepsInUnit in stepsPerUnit)
            {
                StepViz viz = Instantiate(m_StepVizTemplate, m_StepsVizContainer);

                for (int j = 0; j < stepsInUnit; j++)
                {
                    if (currentStepIndex < totalSteps)
                    {
                        if (currentStepIndex == totalSteps - 1)
                        {
                            viz.SetActive();
                        }
                        else
                        {
                            viz.SetCompleted();
                        }
                        currentStepIndex++;
                    }
                    else if (currentMissingStepIndex < totalMissingSteps)
                    {
                        viz.SetMissing();
                        currentMissingStepIndex++;
                    }
                }

                m_StepsVizInstances.Add(viz);
            }
        }
    }
}
