using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScenarioSettings
{
    [CreateAssetMenu(menuName = "Editor/" + nameof(QuestionWith3OptionsDataWrapper), fileName = "new" + nameof(QuestionWith3OptionsDataWrapper))]
    public class QuestionWith3OptionsDataWrapper : ScriptableObject
    {
        public QuestionWith3OptionsData data;
    }
}
