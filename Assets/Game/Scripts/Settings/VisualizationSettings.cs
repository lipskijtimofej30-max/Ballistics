using UnityEngine;

namespace Game.Scripts.Settings
{
    public class VisualizationSettings
    {
        public float Width { get; set; } = 0.1f;
        public Color Color { get; set; } = Color.red;
        public bool VisiblePreview { get; set; } = true;

        public VisualizationSettings Clone() => new VisualizationSettings
        {
            Width = Width,
            Color = Color, 
            VisiblePreview = VisiblePreview
        };
    }
}