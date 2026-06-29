using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class ProjectileFactory : IInitializable
    {
        private Dictionary<ShapeType, Projectile> _prefabs = new(2);
        private GameSettings _gameSettings;

        [Inject]
        private void Construct(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
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
            var prefab = GameObject.Instantiate(_prefabs[_gameSettings.ShapeType]);
            prefab.Initialize(_gameSettings);
            return prefab;
        }
    }
}