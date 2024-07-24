using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScenarioSettings
{
    [CreateAssetMenu(menuName = "Editor/" + nameof(Flip4CardsWithDoubleSideTextDataWrapper), fileName = "new" + nameof(Flip4CardsWithDoubleSideTextDataWrapper))]
    public class Flip4CardsWithDoubleSideTextDataWrapper : ScriptableObject
    {
        public Flip4CardsWithDoubleSideTextData data;
    }
}