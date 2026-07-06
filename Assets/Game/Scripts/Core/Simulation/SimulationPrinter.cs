using Game.Scripts.Infrastructure.Logger;
using System.Collections.Generic;
using Zenject;

namespace Game.Scripts.Core.Simulation
{
    public class SimulationPrinter
    {
        private readonly ILogger _logger;

        [Inject]
        public SimulationPrinter(ILogger logger)
        {
            _logger = logger;
        }

        public void Print(IReadOnlyList<SimulationPoint> points)
        {
            foreach (var point in points)
            {
                _logger.Log(
                    $"t={point.Time:F2} " +
                    $"pos={point.Position} " +
                    $"vel={point.Velocity} " +
                    $"acc={point.Acceleration}");
            }
        }
    }
}