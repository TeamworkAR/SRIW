using System;
using UnityEngine;

namespace Data.ScriptableObjectVariables
{
    // TODO: Generalize this class if more variable types are needed
    [CreateAssetMenu(menuName = "Editor/Variables/" + nameof(TransformVariable))]
    public class TransformVariable : ScriptableObject
    {
        private Transform m_TransformValue = null;

        public Transform TransformValue => m_TransformValue;

        public event Action<TransformVariable> OnValueUpdated = null; 
        
        public void Register(Transform transform)
        {
            if (m_TransformValue != null)
            {
                Debug.LogError($"{transform.name} (root is {transform.root.name}) is trying to register to {this.name} but it already has a value.");
                
                return;
            }

            m_TransformValue = transform;
            
            OnValueUpdated?.Invoke(this);
        }

        public void Unregister(Transform transform)
        {
            if (ReferenceEquals(transform, m_TransformValue) == false)
            {
                Debug.LogError(
                    $"{transform.name} (root is {transform.root.name}) is trying to unregister from {this.name} without being its current value ({m_TransformValue.name} - {m_TransformValue.root.name})");
            }

            m_TransformValue = null;
            
            OnValueUpdated?.Invoke(this);
        }
    }
}