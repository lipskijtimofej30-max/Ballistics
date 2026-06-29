using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class ProjectileFactory : IInitializable
    {
        private Dictionary<ShapeType, Projectile> _prefabs = new(2);
        private ProjectileSettings _settings;

        [Inject]
        private void Construct(ProjectileSettings gameSettings)
        {
            _settings = gameSettings;
        }

        public void Initialize()
        {
            var prefabsArray = Resources.LoadAll<Projectile>("Projectiles");
            foreach (var prefab in prefabsArray)
            {
                _prefabs[prefab.ShapeType] = prefab;
            }
        }

        public Projectile Create()
        {
            var prefab = GameObject.Instantiate(_prefabs[_settings.ShapeType]);
            prefab.Initialize(_settings);
            return prefab;
        }
    }
}