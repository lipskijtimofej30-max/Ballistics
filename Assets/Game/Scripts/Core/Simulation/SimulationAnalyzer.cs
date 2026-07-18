using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Core.Simulation
{
    public class SimulationAnalyzer
    {
        public SimulationSummary Analyze(IReadOnlyList<SimulationPoint> points)
        {
            if (points == null || points.Count == 0) 
                return new SimulationSummary();

            float maxHeight = float.MinValue;
            float timeForMaxHeight = 0f;
            float maxSpeedSq = float.MinValue;

            float totalPath = 0f;
            
            Vector3 previousPosition = points[0].Position;

            for (int i = 0; i < points.Count; i++)
            {
                var p = points[i];

                if (p.Position.y > maxHeight)
                {
                    maxHeight = p.Position.y;
                    timeForMaxHeight = p.Time;
                }

                float currentSpeedSq = p.Velocity.sqrMagnitude;
                if (currentSpeedSq > maxSpeedSq)
                {
                    maxSpeedSq = currentSpeedSq;
                }
                
                if (i > 0)
                {
                    totalPath += Vector3.Distance(p.Position, previousPosition);
                    previousPosition = p.Position;
                }
            }

            var firstPoint = points[0];
            var lastPoint = points[^1];
            
            float displacement = Vector3.Distance(firstPoint.Position, lastPoint.Position);

            return new SimulationSummary
            {
                MaxHeight = maxHeight,
                TimeForMaxHeight = timeForMaxHeight,
            
                MaxSpeed = Mathf.Sqrt(maxSpeedSq), 
            
                Range = lastPoint.Position.x,
                FlightTime = lastPoint.Time,
                
                TotalPath = totalPath,
                Displacement = displacement
            };
        }

        public IReadOnlyList<SimulationComparisons> Compares(SimulationSummary previousSummary, SimulationSummary currentSummary)
        {
            List<SimulationComparisons> comparisonsList = new ()
            {
                new SimulationComparisons("Время полёта", "с", previousSummary.FlightTime, currentSummary.FlightTime),
                new SimulationComparisons("Дальность", "м", previousSummary.Range, currentSummary.Range),
                new SimulationComparisons("Путь", "м", previousSummary.TotalPath, currentSummary.TotalPath),
                new SimulationComparisons("Перемещение", "м", previousSummary.Displacement, currentSummary.Displacement),
                new SimulationComparisons("Макс. Высота", "м", previousSummary.MaxHeight, currentSummary.MaxHeight),
                new SimulationComparisons("Время до вершины", "с", previousSummary.TimeForMaxHeight, currentSummary.TimeForMaxHeight),
                new SimulationComparisons("Макс. скорость", "м/с", previousSummary.MaxSpeed, currentSummary.MaxSpeed)
            };
            return comparisonsList.AsReadOnly();
        }
    }
}