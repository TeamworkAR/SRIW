using System;
using UI;
using UnityEngine;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StartDecisionMaking4GridUIAction : Action
    {
        [SerializeField] private DecisionMakingGridDataWrapper decisionMakingGridData = null;
        public override void Execute()
        {
            MainGUI.Instance.DecisionMaking4GridUI.ShowWithWrapperData(decisionMakingGridData);
        }
    }
}