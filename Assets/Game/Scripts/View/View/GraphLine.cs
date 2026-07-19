using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.View.View
{
    [RequireComponent(typeof(LineRenderer))]
    public class GraphLine : MonoBehaviour
    {
        private const int MaxPoints = 2000;

        private LineRenderer _lineRenderer;
        private IReadOnlyList<Vector2> _optimizedPoints; // точки, которые реально рисуем
        private Vector3[] _positionsCache = new Vector3[100];

        public void Initialize(IReadOnlyList<Vector2> points, Color color)
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.useWorldSpace = false;

            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;

            _optimizedPoints = OptimizePoints(points, MaxPoints);
        }

        public void Draw(Vector2 dataMin, Vector2 dataMax, Vector2 graphSize)
        {
            var drawPoints = _optimizedPoints;
            if (drawPoints == null || drawPoints.Count == 0)
            {
                _lineRenderer.positionCount = 0;
                return;
            }

            if (drawPoints.Count > _positionsCache.Length)
                _positionsCache = new Vector3[drawPoints.Count];

            for (int i = 0; i < drawPoints.Count; i++)
            {
                float nx = Mathf.InverseLerp(dataMin.x, dataMax.x, drawPoints[i].x);
                float ny = Mathf.InverseLerp(dataMin.y, dataMax.y, drawPoints[i].y);
                _positionsCache[i] = new Vector3(nx * graphSize.x, ny * graphSize.y, 0f);
            }

            _lineRenderer.positionCount = drawPoints.Count;
            _lineRenderer.SetPositions(_positionsCache);
        }

        /// <summary>
        /// Если точек слишком много, оставляем каждую N-ю, чтобы уложиться в maxAllowed.
        /// </summary>
        private IReadOnlyList<Vector2> OptimizePoints(IReadOnlyList<Vector2> points, int maxAllowed)
        {
            if (points == null || points.Count <= maxAllowed)
                return points;

            int step = Mathf.CeilToInt((float)points.Count / maxAllowed);
            var result = new List<Vector2>(maxAllowed);

            for (int i = 0; i < points.Count; i += step)
                result.Add(points[i]);

            return result;
        }
    }
}