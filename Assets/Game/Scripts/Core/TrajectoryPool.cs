using System.Collections.Generic;
using Game.Scripts.View.View;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class TrajectoryPool
    {
        private readonly TrajectoryRenderer _prefab;
        private readonly DiContainer _container;
        private List<TrajectoryRenderer> _active = new();

        [Inject]
        public TrajectoryPool(DiContainer container)
        {
            _container = container;
            _prefab = Resources.Load<TrajectoryRenderer>("Prefabs/TrajectoryRenderer");
        }

        public TrajectoryRenderer Rent()
        {
            var instance = _container.InstantiatePrefabForComponent<TrajectoryRenderer>(_prefab);
            _active.Add(instance);
            return instance;
        }

        public void ClearAll()
        {
            foreach (var instance in _active)
                GameObject.Destroy(instance.gameObject);
            
            _active.Clear();
        }
    }
}