using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Editor/" + nameof(DecisionMakingGridDataWrapper), fileName = "new" + nameof(DecisionMakingGridDataWrapper))]
public class DecisionMakingGridDataWrapper : ScriptableObject
{
    [SerializeField] private DecisionMakingGridData decisionMakingGridData = null;

    public DecisionMakingGridData DecisionMakingGridData => decisionMakingGridData;
}
