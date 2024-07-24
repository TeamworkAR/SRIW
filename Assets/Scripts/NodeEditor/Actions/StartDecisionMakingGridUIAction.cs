using System;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartDecisionMakingGridUIAction : Action
    {
        [SerializeField] private DecisionMakingGridDataWrapper decisionMakingGridData = null;
        public override void Execute()
        {
            MainGUI.Instance.DecisionMakingGridUI.ShowWithWrapperData(decisionMakingGridData);
        }
    }
}