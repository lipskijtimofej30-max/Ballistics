using System.Collections.Generic;
using Game.Scripts.Core.Simulation;
using UnityEngine;

namespace Assets.Game.Scripts.Core.Graphics
{
    public class RangeTimeGraphDataSource : IGraphDataSource
    {
        public bool IsVisible { get; set; } =  true;
        public string XAxisLabel { get; } = "Время, с";
        public string YAxisLabel { get; } = "X, м";
        public string DisplayName { get; } = "Дальность от времени";
        public Vector2 MinBound { get; }
        public Vector2 MaxBound { get; }
        
        private readonly List<Vector2> _cachedPoints;

        public RangeTimeGraphDataSource(SimulationRun run)
        { 
            _cachedPoints = new List<Vector2>(run.Points.Count);
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;

            foreach (var point in run.Points)
            {
                Vector2 p = new Vector2(point.Time, point.Position.x);
                _cachedPoints.Add(p);
                
                if (p.x < minX) minX = p.x;
                if (p.x > maxX) maxX = p.x;
                if (p.y < minY) minY = p.y;
                if (p.y > maxY) maxY = p.y;
            }
            
            MinBound = new Vector2(minX, minY);
            MaxBound = new Vector2(maxX, maxY);
        }
        
        public IReadOnlyList<Vector2> GetPoints() => _cachedPoints;
    }
}