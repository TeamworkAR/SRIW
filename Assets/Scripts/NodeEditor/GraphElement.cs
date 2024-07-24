using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    public abstract class GraphElement : ScriptableObject
    {
        // Guid made not editor only to be used to save user state if a scenario is interrupted
        [SerializeField] private string m_Guid = string.Empty;

        public string Guid
        {
            get
            {
                if (string.IsNullOrEmpty(m_Guid))
                {
#if UNITY_EDITOR
                    // TODO: Consider using an helper method to ensure consistency when dealing with GUIDs strings
                    m_Guid = GUID.Generate().ToString();
#endif 
                }
                
                // TODO: Throw an exception if _guid is null or empty
                return m_Guid;
            }
        }
    }
}