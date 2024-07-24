using UnityEngine.UIElements;

namespace NodeEditor.Editor
{
    public class SplitView : TwoPaneSplitView
    {
        /// <summary>
        /// Nested class needed for GraphView
        /// </summary>
        public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits>
        {
        }  
    }
}