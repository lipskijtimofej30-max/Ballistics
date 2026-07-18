using System.Collections.Generic;
using Game.Scripts.Core.Simulation;
using Game.Scripts.Infrastructure;
using UnityEngine;

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

        private void Awake()
        {
            _lineRenderer.positionCount = 0;
            _lineRenderer.enabled = false;
        }

        public void AppendPoint(Vector3 position)
        {
            _stepsSinceLastPoint++;

            if (_stepsSinceLastPoint < _liveStride)
                return;

            _stepsSinceLastPoint = 0;

            if (_lineRenderer.positionCount >= LiveMaxPoints)
                Compact();

            _liveBuffer.Add(position);
            _framesSinceUpdate++;

            if (_framesSinceUpdate >= 2)
            {
                FlushBuffer();
                _framesSinceUpdate = 0;
            }
        }

        public void FlushBuffer()
        {
            if (_liveBuffer.Count == 0)
                return;

            int startIndex = _lineRenderer.positionCount;
            int newSize = startIndex + _liveBuffer.Count;
            _lineRenderer.positionCount = newSize;

            for (int i = 0; i < _liveBuffer.Count; i++)
                _lineRenderer.SetPosition(startIndex + i, _liveBuffer[i]);
            _liveBuffer.Clear();
        }

        private void Compact()
        {
            int oldCount = _lineRenderer.positionCount;
            if (oldCount == 0)
                return;

            Vector3[] existing = new Vector3[oldCount];
            _lineRenderer.GetPositions(existing);

            int newCount = 0;
            for (int i = 0; i < oldCount; i += 2)
                existing[newCount++] = existing[i];

            _lineRenderer.positionCount = newCount;
            for(int i = 0; i < newCount; i++)
                _lineRenderer.SetPosition(i, existing[i]);
            _liveStride *= 2; 
        }

        public void DrawFull(IReadOnlyList<SimulationPoint> points)
        {
            int step = Mathf.Max(1, points.Count / PreviewTargetPoints);

            var positions = new Vector3[(points.Count + step - 1) / step];
            for (int i = 0, j = 0; i < points.Count; i += step)
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

        public void SetColor(int index, float alpha = 1)
        {
            var color = ExperimentPalette.Palette[index % ExperimentPalette.Palette.Length];
            color.a = alpha;
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