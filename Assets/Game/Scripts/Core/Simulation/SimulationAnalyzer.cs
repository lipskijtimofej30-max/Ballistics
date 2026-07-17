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
    }
}