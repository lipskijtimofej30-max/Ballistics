using Assets.Game.Scripts.Core.Calculator;
using Assets.Game.Scripts.Settings;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class ProjectileFactory : IInitializable
    {
        private Dictionary<ShapeType, Projectile> _prefabs = new(2);
        private ProjectileSettings _projectileSettings;
        private SimulationSettings _simulationSettings;
        private LaunchVelocityCalculator _velocityCalculator;
        private MassCalculator _massCalculator;
        private CrossSectionalAreaCalculator _crossSectionalAreaCalculator;

        [Inject]
        private void Construct(ProjectileSettings gameSettings, SimulationSettings simulationSettings,
            LaunchVelocityCalculator velocityCalculator, MassCalculator massCalculator, CrossSectionalAreaCalculator areaCalculator)
        {
            _projectileSettings = gameSettings;
            _velocityCalculator = velocityCalculator;
            _massCalculator = massCalculator;
            _simulationSettings = simulationSettings;
            _crossSectionalAreaCalculator = areaCalculator;
        }

        public void Initialize()
        {
            var prefabsArray = Resources.LoadAll<Projectile>("Projectiles");
            foreach (var prefab in prefabsArray)
                _prefabs[prefab.ShapeType] = prefab;
        }

        public Projectile Create()
        {
            var prefab = GameObject.Instantiate(_prefabs[_projectileSettings.ShapeType]);
            prefab.Initialize(_projectileSettings, _simulationSettings, _velocityCalculator, _massCalculator, _crossSectionalAreaCalculator);
            return prefab;
        }
    }
}