using System.Collections.Generic;
using Game.Scripts.Core.Simulation;
using UnityEngine;

namespace Game.Scripts.View.View
{
    public class TrajectoryRenderer : MonoBehaviour
    {
        private static Color[] Pallette =
        {
            Color.cyan, Color.yellow, Color.magenta, Color.green, Color.red
        };
        
        [SerializeField] private LineRenderer _lineRenderer;

        public void AppendPoint(Vector3 position)
        {
            int index = _lineRenderer.positionCount;
            _lineRenderer.positionCount = index + 1;
            _lineRenderer.SetPosition(index, position);
        }

        public void DrawFull(IReadOnlyList<SimulationPoint> points)
        {
            _lineRenderer.positionCount = points.Count;
            for (int i = 0; i < points.Count; i++)
                _lineRenderer.SetPosition(i, points[i].Position);
        }

        public void SetColor(int index)
        {
            var color = Pallette[index % Pallette.Length];
            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
        }
        
        public void Clear() => _lineRenderer.positionCount = 0;
    }
}