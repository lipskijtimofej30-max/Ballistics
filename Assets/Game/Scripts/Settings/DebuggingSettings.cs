using System;
using Assets.Game.Scripts.Settings;
using Game.Scripts.Core;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Settings
{
    public class DebuggingSettings : MonoBehaviour
    {
        private ProjectileSettings _projectileSettings;
        private EnvironmentSettings _environmentSettings;
        private SimulationSettings _simulationSettings;

        private float _timer;

        [Inject]
        private void Construct(ProjectileSettings projectileSettings, EnvironmentSettings environmentSettings, SimulationSettings simulationSettings)
        {
            _projectileSettings = projectileSettings;
            _environmentSettings = environmentSettings;
            _simulationSettings = simulationSettings;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > 5f)
            {
                _timer = 0f;
                Debug.LogWarning($"Shape: {_projectileSettings.ShapeType}; Density: {_projectileSettings.Density}; Size: {_projectileSettings.Size}\n" +
                                 $"Initial Position: {_simulationSettings.InitialPosition}; Initial Speed {_simulationSettings.InitialSpeed}; Angle {_simulationSettings.LaunchAngle}\n" +
                                 $"Gravity: {_environmentSettings.Gravity}; Wind Velocity {_environmentSettings.WindVelocity}; Air Density: {_environmentSettings.AirDensity}; Air Enabled {_environmentSettings.AirResistanceEnabled}");
            }
        }
    }
}