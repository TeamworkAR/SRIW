using System;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace NodeEditor.Editor
{
    public sealed class EdgeView : Edge, IDisposable
    {
        public static Action<EdgeView> onNodeViewSelected;
        
        private Edges.Edge m_Data;

        public Edges.Edge Data {
            set
            {
                if (m_Data != null)
                {
                    // TODO: Add meaningful error
                    Debug.LogError("");
                    
                    return;
                }

                m_Data = value;
                m_Data.onDataChanged += OnDataChanged;

                viewDataKey = value.Guid;
            }
            get => m_Data;
        }

        private void OnDataChanged()
        {
            UpdateEdgeLabel();
        }

        public override void OnSelected()
        {
            base.OnSelected();
            UpdateEdgeLabel();
            onNodeViewSelected?.Invoke(this);
        }
        
        private void UpdateEdgeLabel()
        {
            string labelText = $"{m_Data}";
            Label label = this.Q<Label>(m_Data.name);
            if (label == null)
            {
                label = new Label(labelText);
                label.name = m_Data.name;
                this.Add(label);
            }
            
            // Adjust the style of the label
            label.style.fontSize = 12;
            
            //label.style.color = new Color(0, 0, 0); // Black color, change as needed

            // Align the text to the center
            label.style.alignSelf = Align.Center;
            label.style.unityTextAlign = TextAnchor.MiddleCenter;

            // Add padding or other styling as needed
            label.style.paddingLeft = 10;
            label.style.paddingRight = 10;

            // TODO: slight height offset
            // TODO: Update position when a node is moved
            label.style.left = (edgeControl.controlPoints[0] + (edgeControl.controlPoints[edgeControl.controlPoints.Length-1]-edgeControl.controlPoints[0])/2).x;
            label.style.top = (edgeControl.controlPoints[0] + (edgeControl.controlPoints[edgeControl.controlPoints.Length-1]-edgeControl.controlPoints[0])/2).y;
        }

        public void Dispose()
        {
            if (m_Data != null)
            {
                m_Data.onDataChanged -= OnDataChanged;
            }
        }
    }
}