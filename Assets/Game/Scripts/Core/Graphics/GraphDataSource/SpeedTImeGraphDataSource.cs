using System.Collections.Generic;
using Game.Scripts.Core.Simulation;
using UnityEngine;

namespace Assets.Game.Scripts.Core.Graphics
{
    public class SpeedTImeGraphDataSource : IGraphDataSource
    {
        private readonly SimulationRun _run;

        public string XAxisLabel { get; } = "Время, с";
        public string YAxisLabel { get; } = "Скорость, м/с";
        public string DisplayName { get; } = "v(t)";

        public SpeedTImeGraphDataSource(SimulationRun run) => _run = run;
        public List<Vector2> GetPoints()
        {
            List<Vector2> points = new List<Vector2>(_run.Points.Count);
            foreach (var point in _run.Points)
                points.Add(new Vector2(point.Time, point.Velocity.magnitude));
            return points;
        }
    }
}