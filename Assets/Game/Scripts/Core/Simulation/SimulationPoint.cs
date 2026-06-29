using UnityEngine;

namespace Game.Scripts.Core.Simulation
{
    public class SimulationPoint
    {
        public float Time { get; set; }

        public Vector3 Position { get; set; }

        public Vector3 Velocity { get; set; }

        public Vector3 Acceleration { get; set; }

        public Vector3 TotalForce { get; set; }
    }
}