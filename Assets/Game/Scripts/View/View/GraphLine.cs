using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.View.View
{
    [RequireComponent(typeof(LineRenderer))]
    public class GraphLine : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private List<Vector2> _points;

        public void Initialize(List<Vector2> points, Color color)
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.useWorldSpace = false;
        
            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
        
            _points = points;
        }

        public void Draw(Vector2 dataMin, Vector2 dataMax, Vector2 graphSize)
        {
            if (_points == null || _points.Count == 0)
            {
                _lineRenderer.positionCount = 0;
                return;
            }

            var positions = new Vector3[_points.Count];
            for (int i = 0; i < _points.Count; i++)
            {
                float nx = Mathf.InverseLerp(dataMin.x, dataMax.x, _points[i].x);
                float ny = Mathf.InverseLerp(dataMin.y, dataMax.y, _points[i].y);
            
                positions[i] = new Vector3(nx * graphSize.x, ny * graphSize.y, 0f);
            }
        
            _lineRenderer.positionCount = _points.Count;
            _lineRenderer.SetPositions(positions);
        }
    }
}