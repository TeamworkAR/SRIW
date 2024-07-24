using UnityEngine;

namespace Data.ScriptableObjectVariables
{
    // TODO: Generalize this class if more variable types are needed
    public class TransformVariableBinder : MonoBehaviour
    {
        [SerializeField] private TransformVariable m_TransformVariable = null;

        private void OnEnable()
        {
            m_TransformVariable.Register(transform);
        }

        private void OnDisable()
        {
            m_TransformVariable.Unregister(transform);
        }
    }
}