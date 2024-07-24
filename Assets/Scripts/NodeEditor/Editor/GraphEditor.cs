using NodeEditor.Graphs;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor.Editor
{
    public class GraphEditor : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        private GraphEditorGraphView m_GraphView = null;

        [MenuItem(nameof(GraphEditor) + "/Editor")]
        public static void ShowExample()
        {
            GraphEditor wnd = GetWindow<GraphEditor>();
            wnd.titleContent = new GUIContent("NodeEditorWindow");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
        
            // Instantiate UXML
            VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
            root.Add(labelFromUXML);

            m_GraphView = root.Q<GraphEditorGraphView>();
            
            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            if (Selection.activeObject is Graph graph)
            {
                m_GraphView.PopulateView(graph);
            }
        }
    }
}
