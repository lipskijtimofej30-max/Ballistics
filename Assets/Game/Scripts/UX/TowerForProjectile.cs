using Assets.Game.Scripts.Settings;
using DG.Tweening;
using Game.Scripts.Core;
using Game.Scripts.Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.UX
{
    public class TowerForProjectile : MonoBehaviour
    {
        private SimulationSettings _simulationSettings;
        private Simulator _simulator;
        private SignalBus _signalBus;

        private GameObject _currentTower;

        [Inject]
        private void Construct(SimulationSettings simulationSettings, Simulator simulator, SignalBus signalBus)
        {
            _simulationSettings = simulationSettings;
            _simulator = simulator;
            _signalBus = signalBus;

            _signalBus.Subscribe<ProjectileSpawnedSignal>(Spawn);
        }

        private void Spawn()
        {
            if (_currentTower == null && _simulator.CurrentBody != null)
            {
                _currentTower = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                SetTransform();
            }
            SetTransform();
        }

        private void SetTransform()
        {
            _currentTower.transform.DOScale(new Vector3(1f, _simulationSettings.InitialPosition.y/2f, 1f), 0.1f);
            _currentTower.transform.position = new Vector3(
                _simulator.CurrentBody.transform.position.x,
                _simulationSettings.InitialPosition.y/2f - _simulator.CurrentBody.transform.localScale.x/2f,
                _simulator.CurrentBody.transform.position.z);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<ProjectileSpawnedSignal>(Spawn);
        }
    }
}