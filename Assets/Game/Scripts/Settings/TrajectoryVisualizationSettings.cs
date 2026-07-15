using UnityEngine;

namespace Game.Scripts.Settings
{
    public class TrajectoryVisualizationSettings
    {
        public float Width { get; set; } = 0.1f;
        public Color Color { get; set; } = Color.red;
        public bool VisiblePreview { get; set; } = true;

        public TrajectoryVisualizationSettings Clone() => new TrajectoryVisualizationSettings
        {
            Width = Width,
            Color = Color, 
            VisiblePreview = VisiblePreview
        };
    }
}