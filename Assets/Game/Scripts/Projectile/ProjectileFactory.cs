using Assets.Game.Scripts.Core.Calculator;
using Assets.Game.Scripts.Settings;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class ProjectileFactory : IInitializable
    {
        private Dictionary<ShapeType, ProjectileBody> _prefabs = new(2);
        
        private ProjectileSettings _projectileSettings;
        private SimulationSettings _simulationSettings;
        private VelocityCalculator _velocityCalculator;
        private MassCalculator _massCalculator;
        private CrossSectionalAreaCalculator _crossSectionalAreaCalculator;

        [Inject]
        private void Construct(ProjectileSettings gameSettings, SimulationSettings simulationSettings,
            VelocityCalculator velocityCalculator, MassCalculator massCalculator, CrossSectionalAreaCalculator areaCalculator)
        {
            _projectileSettings = gameSettings;
            _velocityCalculator = velocityCalculator;
            _massCalculator = massCalculator;
            _simulationSettings = simulationSettings;
            _crossSectionalAreaCalculator = areaCalculator;
        }

        public void Initialize()
        {
            var prefabsArray = Resources.LoadAll<ProjectileBody>("Projectiles");
            foreach (var prefab in prefabsArray)
                _prefabs[prefab.ShapeType] = prefab;
        }

        public ProjectileState CreateState()
        {
            var shapeType = _projectileSettings.ShapeType;
            float size = _projectileSettings.Size;
            float density = _projectileSettings.Density;
 
            float mass = _massCalculator.GetMass(shapeType, density, size);
            float area = _crossSectionalAreaCalculator.GetCrossSectionalArea(shapeType, size);
            float dragCoefficient = GetDragCoefficient(shapeType);
 
            Vector3 velocity = _velocityCalculator.GetVelocity(_simulationSettings.InitialSpeed, _simulationSettings.LaunchAngle);
            Vector3 position = _simulationSettings.InitialPosition;
 
            return new ProjectileState(shapeType, size, density, position, velocity, mass, area, dragCoefficient);
        }

        public ProjectileBody CreateBody()
        {
            var shapeType = _projectileSettings.ShapeType;
            var prefab = GameObject.Instantiate(_prefabs[shapeType]);
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