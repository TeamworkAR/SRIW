using UnityEngine.UIElements;

namespace NodeEditor.Editor
{
    // TODO: Resume creating a custom inspector for edges
    public class InspectorView : VisualElement
    {
        private UnityEditor.Editor m_Editor;
        
        /// <summary>
        /// Nested class needed for GraphView
        /// </summary>
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits>
        {
        }
        
        /// <summary>
        /// Constructor needed for GraphView
        /// </summary>
        public InspectorView()
        {
            // TODO: I can do better than this
            NodeView.onNodeViewSelected -= UpdateSelection;
            NodeView.onNodeViewSelected += UpdateSelection;
            
            // TODO: I can do better than this
            EdgeView.onNodeViewSelected -= UpdateSelection;
            EdgeView.onNodeViewSelected += UpdateSelection;
        }
        
        public void UpdateSelection(EdgeView edgeView)
        {
            // TODO: Clear the editor when a node is destroyed
            Clear();
            
            UnityEngine.Object.DestroyImmediate(m_Editor);
            // TODO: This requires to derive from ScriptableObject. 
            // TODO: I would really like to avoid that, is it possible to implement a map-based approach but it would be very labour intensive
            m_Editor = UnityEditor.Editor.CreateEditor(edgeView.Data);
            
            IMGUIContainer container = new IMGUIContainer(() => { m_Editor.OnInspectorGUI(); });
            Add(container);
        }
        
        public void UpdateSelection(NodeView nodeView)
        {
            // TODO: Clear the editor when a node is destroyed
            Clear();
            
            UnityEngine.Object.DestroyImmediate(m_Editor);
            // TODO: This requires to derive from ScriptableObject. 
            // TODO: I would really like to avoid that, is it possible to implement a map-based approach but it would be very labour intensive
            m_Editor = UnityEditor.Editor.CreateEditor(nodeView.Data);
            
            IMGUIContainer container = new IMGUIContainer(() => { m_Editor.OnInspectorGUI(); });
            Add(container);
        }
    }
}