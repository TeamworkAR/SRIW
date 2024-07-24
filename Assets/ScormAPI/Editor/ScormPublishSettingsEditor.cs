using UnityEditor;
using UnityEngine;

namespace Scorm.Editor {

    [CustomEditor(typeof(ScormPublishSettings))]
    public class ScormPublishSettingsEditor : UnityEditor.Editor {

        private ScormPublishSettings settings;

        void OnEnable() {
            settings = (ScormPublishSettings)target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Select zip path")) {
                string title = "Save Scorm as ZIP";
                string directory = "";
                string defaultName = "Scorm";
                string extension = "zip";

                settings.ZipFilePath = EditorUtility.SaveFilePanel(title, directory, defaultName, extension);

                EditorUtility.SetDirty(settings);
            }

            EditorGUILayout.EndHorizontal();
        }

    }

}
