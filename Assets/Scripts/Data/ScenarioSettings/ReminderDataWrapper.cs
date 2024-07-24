using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScenarioSettings
{
    [CreateAssetMenu(menuName = "Editor/" + nameof(ReminderDataWrapper), fileName = "new" + nameof(ReminderDataWrapper))]
    public class ReminderDataWrapper : ScriptableObject
    {
        public ReminderData Data;
    }
}