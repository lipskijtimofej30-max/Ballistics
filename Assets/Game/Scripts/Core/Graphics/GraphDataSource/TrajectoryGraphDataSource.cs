using System.Collections.Generic;
using Game.Scripts.Core.Simulation;
using UnityEngine;

namespace Assets.Game.Scripts.Core.Graphics
{
    public class TrajectoryGraphDataSource : IGraphDataSource
    {
        private readonly SimulationRun _run;
        
        public string XAxisLabel { get; } = "Дальность, м";
        public string YAxisLabel { get; } = "Высота, м";
        public string DisplayName { get; } = "Траектория полёта";

        public TrajectoryGraphDataSource(SimulationRun run) => _run = run;
        
        public List<Vector2> GetPoints()
        {
            List<Vector2> points = new List<Vector2>(_run.Points.Count);
            foreach (var point in _run.Points)
                points.Add(new Vector2(point.Position.x, point.Position.y));
            return points;
        }

        
    }
}