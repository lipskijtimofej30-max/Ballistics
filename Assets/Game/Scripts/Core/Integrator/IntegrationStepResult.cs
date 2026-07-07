
using UnityEngine;

namespace Game.Scripts.Core
{
    public readonly struct IntegrationStepResult
    {
        public Vector3 Acceleration { get; }
        public Vector3 TotalForce { get; }
 
        public IntegrationStepResult(Vector3 acceleration, Vector3 totalForce)
        {
            Acceleration = acceleration;
            TotalForce = totalForce;
        }
    }
}