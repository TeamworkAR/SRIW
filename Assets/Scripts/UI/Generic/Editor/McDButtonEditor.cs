using UnityEditor;
using UnityEditor.UI;

namespace UI.Generic.Editor
{
    [CustomEditor(typeof(McDButton), true)]
    [CanEditMultipleObjects]
    public class McDButtonEditor : ButtonEditor
    {
        private SerializedProperty m_ButtonColorsOverride = null;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_ButtonColorsOverride = serializedObject.FindProperty("m_Override");
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_ButtonColorsOverride);
            serializedObject.ApplyModifiedProperties();
        }
    }
}