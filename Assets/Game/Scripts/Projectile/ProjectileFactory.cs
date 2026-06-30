using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class ProjectileFactory : IInitializable
    {
        private Dictionary<ShapeType, Projectile> _prefabs = new(2);
        private ProjectileSettings _settings;
        private LaunchVelocityCalculator _velocityCalculator;
        private MassCalculator _massCalculator;

        [Inject]
        private void Construct(ProjectileSettings gameSettings, LaunchVelocityCalculator velocityCalculator, MassCalculator massCalculator)
        {
            _settings = gameSettings;
            _velocityCalculator = velocityCalculator;
            _massCalculator = massCalculator;
        }

        public void Initialize()
        {
            var prefabsArray = Resources.LoadAll<Projectile>("Projectiles");
            foreach (var prefab in prefabsArray)
                _prefabs[prefab.ShapeType] = prefab;
        }

        public Projectile Create()
        {
            var prefab = GameObject.Instantiate(_prefabs[_settings.ShapeType]);
            prefab.Initialize(_settings,  _velocityCalculator, _massCalculator);
            return prefab;
        }
    }
}