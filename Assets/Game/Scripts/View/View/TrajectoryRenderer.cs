using System.Collections.Generic;
using Game.Scripts.Core;
using Game.Scripts.Core.Simulation;
using UnityEngine;
using Zenject;

namespace Game.Scripts.View.View
{
    public class TrajectoryRenderer : MonoBehaviour
    {
        private const int LiveMaxPoints = 3500;
        private const int PreviewTargetPoints = 1000;
   
        [SerializeField] private LineRenderer _lineRenderer;
        
        private List<Vector3> _liveBuffer = new();
        private int _framesSinceUpdate;
        private int _liveStride = 1;
        private int _stepsSinceLastPoint;
        
        public void AppendPoint(Vector3 position)
        {    
            _stepsSinceLastPoint++;
            if (_lineRenderer.positionCount >= LiveMaxPoints) return;
            _stepsSinceLastPoint = 0;
            
            _liveBuffer.Add(position);
            _framesSinceUpdate++;

            if (_framesSinceUpdate >= 2)
            {
                FlushBuffer();
                _framesSinceUpdate = 0;
            }
            
            if(_liveBuffer.Count >= LiveMaxPoints)
                Compact();
        }

        public void FlushBuffer()
        {
            if (_liveBuffer.Count == 0) return;

            int startIndex = _lineRenderer.positionCount;
            _lineRenderer.positionCount = startIndex + _liveBuffer.Count;

            for (int i = 0; i < _liveBuffer.Count; i++)
                _lineRenderer.SetPosition(startIndex + i, _liveBuffer[i]);

            _liveBuffer.Clear();
        }
        
        private void Compact()
        {
            var existing = new Vector3[_lineRenderer.positionCount];
            _lineRenderer.GetPositions(existing);

            int newCount = 0;
            for (int i = 0; i < existing.Length; i += 2)
                existing[newCount++] = existing[i];

            _lineRenderer.positionCount = newCount;
            for (int i = 0; i < newCount; i++)
                _lineRenderer.SetPosition(i, existing[i]);

            _liveStride *= 2; // дальше берём точки в два раза реже
        }
        

        public void DrawFull(IReadOnlyList<SimulationPoint> points)
        {
            int step = Mathf.Max(1, points.Count / PreviewTargetPoints);
            
            var positions = new Vector3[(points.Count + step - 1)/step];
            for (int i = 0, j = 0; i < points.Count; i+= step)
                positions[j++] = points[i].Position;
            
            _lineRenderer.positionCount = positions.Length;
            _lineRenderer.SetPositions(positions);
        }

        public void SetSettings(float width, Color color)
        {
            _lineRenderer.startWidth = width;
            _lineRenderer.endWidth = width;
            
            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
        }
        
        public void SetVisible(bool visible) => _lineRenderer.enabled = visible;
        
        public void Clear()
        {
            _lineRenderer.positionCount = 0;
            _liveBuffer.Clear();
            _liveStride = 1;
            _stepsSinceLastPoint = 0;
        }
    }
}