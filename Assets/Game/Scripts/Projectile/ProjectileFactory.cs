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
            var shapeType = _projectileSettings.ShapeType;
            var prefab = GameObject.Instantiate(_prefabs[shapeType]);

            float mass = _massCalculator.GetMass(shapeType, _projectileSettings.Density, _projectileSettings.Size);
            float area = _crossSectionalAreaCalculator.GetCrossSectionalArea(shapeType, _projectileSettings.Size);
            float dragCoefficient = GetDragCoefficient(shapeType);

            Vector3 velocity = _velocityCalculator.GetVelocity();
            Vector3 position = _simulationSettings.InitialPosition;
            
            prefab.Initialize(position, velocity, mass, area, dragCoefficient);
            return prefab;
        }

        private float GetDragCoefficient(ShapeType shapeType)
        {
            return shapeType switch
            {
                ShapeType.Cube => 1.05f,
                ShapeType.Sphere => 0.47f,
                _ => 0.5f
            };
        }
    }
}