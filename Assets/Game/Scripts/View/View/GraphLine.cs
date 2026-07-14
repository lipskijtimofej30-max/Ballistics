using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.View.View
{
    [RequireComponent(typeof(LineRenderer))]
    public class GraphLine : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private IReadOnlyList<Vector2> _points;
        private Vector3[] _positionsCache = new Vector3[100];

        public void Initialize(IReadOnlyList<Vector2> points, Color color)
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
            
            if (_points.Count > _positionsCache.Length)
                _positionsCache = new Vector3[_points.Count * 2];

            for (int i = 0; i < _points.Count; i++)
            {
                float nx = Mathf.InverseLerp(dataMin.x, dataMax.x, _points[i].x);
                float ny = Mathf.InverseLerp(dataMin.y, dataMax.y, _points[i].y);
            
                _positionsCache[i] = new Vector3(nx * graphSize.x, ny * graphSize.y, 0f);
            }
        
            _lineRenderer.positionCount = _points.Count;
            _lineRenderer.SetPositions(_positionsCache);
        }
    }
}