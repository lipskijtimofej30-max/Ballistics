using System.Collections.Generic;
using System.Linq;

namespace Game.Scripts.Core.Simulation
{
    public class SimulationAnalyzer
    {
        public SimulationSummary Analyze(IReadOnlyList<SimulationPoint> points)
        {
            if (points.Count == 0) return new SimulationSummary();

            return new SimulationSummary
            {
                MaxHeight = points.Max(p => p.Position.y),
                MaxSpeed = points.Max(p => p.Velocity.magnitude),
                Range = points[^1].Position.x,
                FlightTime = points[^1].Time
            };
        }
    }
}