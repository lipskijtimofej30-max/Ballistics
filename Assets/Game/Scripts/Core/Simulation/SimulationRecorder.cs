using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Core.Simulation
{
    public class SimulationRecorder
    {
        public IReadOnlyList<SimulationPoint> Points => _points;

        private readonly List<SimulationPoint> _points = new();

        private float _currentTime;

        public void Clear()
        {
            _points.Clear();
            _currentTime = 0f;
        }

        public void Record(
            Vector3 position,
            Vector3 velocity,
            Vector3 acceleration,
            Vector3 force)
        {
            _points.Add(new SimulationPoint
            {
                Time = _currentTime,
                Position = position,
                Velocity = velocity,
                Acceleration = acceleration,
                TotalForce = force
            });

            _currentTime += Time.fixedDeltaTime;
        }
    }
}