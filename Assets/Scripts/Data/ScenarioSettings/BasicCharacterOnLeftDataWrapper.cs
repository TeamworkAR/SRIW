using UnityEngine;

namespace Data.ScenarioSettings
{
    [CreateAssetMenu(menuName = "Editor/" + nameof(BasicCharacterOnLeftDataWrapper), fileName = "new" + nameof(BasicCharacterOnLeftDataWrapper))]
    public class BasicCharacterOnLeftDataWrapper : ScriptableObject
    { 
        [SerializeField] private BasicCharacterOnLeftData basicCharacterOnLeftData = null;

        public BasicCharacterOnLeftData BasicCharacterOnLeft => basicCharacterOnLeftData;

    }
}

