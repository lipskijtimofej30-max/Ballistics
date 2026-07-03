using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Core.Simulation
{
    public class SimulationRun
    {
        private readonly List<SimulationPoint> _points = new();
        private float _elapsedTime;
        
        public IReadOnlyList<SimulationPoint> Points => _points;

        public void AddPoint(Vector3 position, Vector3 velocity, Vector3 acceleration, Vector3 totalForce,
            float deltaTime)
        {
            _points.Add(new SimulationPoint
            {
                Time = _elapsedTime,
                Position = position,
                Velocity = velocity,
                Acceleration = acceleration,
                TotalForce = totalForce
            });
            _elapsedTime += deltaTime;
        }
    }
}