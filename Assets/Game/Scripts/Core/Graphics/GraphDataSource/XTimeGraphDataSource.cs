using System.Collections.Generic;
using Game.Scripts.Core.Simulation;
using UnityEngine;

namespace Assets.Game.Scripts.Core.Graphics
{
    public class XTimeGraphDataSource : IGraphDataSource
    {
        private  readonly SimulationRun _run;

        public string XAxisLabel { get; } = "Время, с";
        public string YAxisLabel { get; } = "X, м";
        public string DisplayName { get; } = "x(t)";

        public XTimeGraphDataSource(SimulationRun run) => _run = run;
        
        public List<Vector2> GetPoints()
        {
            List<Vector2> points = new List<Vector2>(_run.Points.Count);
            foreach (var point in _run.Points)
                points.Add(new Vector2(point.Time, point.Position.x));
            return points;
        }
    }
}