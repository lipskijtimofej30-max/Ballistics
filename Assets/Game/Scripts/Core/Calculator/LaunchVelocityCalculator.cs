using Assets.Game.Scripts.Settings;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class LaunchVelocityCalculator
    {
        private SimulationSettings _simulationSettings;

        public LaunchVelocityCalculator(SimulationSettings simulationSettings)
        {
            _simulationSettings = simulationSettings;
        }

        public Vector3 GetVelocity()
        {
            float angle = Mathf.Deg2Rad * _simulationSettings.LaunchAngle;
            var velocity = new Vector3(
                _simulationSettings.InitialSpeed * Mathf.Cos(angle),
                _simulationSettings.InitialSpeed * Mathf.Sin(angle),
                0f);
            return velocity;
        }
    }
}