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
        public TrajectoryPool(DiContainer container,TrajectoryRenderer prefab)
        {
            _container = container;
            _prefab = prefab;
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
        public void Release(TrajectoryRenderer instance)
        {
            if (instance != null && _active.Contains(instance))
            {
                _active.Remove(instance);
                GameObject.Destroy(instance.gameObject);
            }
        }
    }
}