using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Core.Simulation
{
    public class SimulationPrinter
    {
        public void Print(IReadOnlyList<SimulationPoint> points)
        {
            foreach (var point in points)
            {
                Debug.Log(
                    $"t={point.Time:F2} " +
                    $"pos={point.Position} " +
                    $"vel={point.Velocity} " +
                    $"acc={point.Acceleration}");
            }
        }
    }
}