using Assets.Game.Scripts.Infrastructure.Signals;
using Assets.Game.Scripts.Settings;
using DG.Tweening;
using Game.Scripts.Core;
using Game.Scripts.Infrastructure.Signals;
using UnityEngine;
using Zenject;
using ILogger = Game.Scripts.Infrastructure.Logger.ILogger;

namespace Assets.Game.Scripts.UX
{
    public class LaunchStand : MonoBehaviour
    {
        [Header("Stand Parts")] 
        [SerializeField] private Transform _base;
        [SerializeField] private Transform _pole;
        [SerializeField] private Transform _platform;

        [Header("Settings")] 
        [SerializeField] private float _baseHeight = 0.2f;
        [SerializeField] private float _platformHeight = 0.1f;
        [SerializeField] private float _animationDuration = 0.2f;

        private SimulationSettings _simulationSettings;
        private Simulator _simulator;
        private SignalBus _signalBus;
        private ILogger _logger;

        [Inject]
        private void Construct(
            SimulationSettings simulationSettings,
            Simulator simulator,
            SignalBus signalBus,
            ILogger logger)
        {
            _simulationSettings = simulationSettings;
            _simulator = simulator;
            _signalBus = signalBus;
            _logger = logger;    
            
            _signalBus.Subscribe<ProjectileSpawnedSignal>(UpdateStand);
            _signalBus.Subscribe<SimulationSettingsChangedSignal>(UpdateStand);
        }

        private void Start()
        {
            UpdateStand();
        }

        private void UpdateStand()
        {
            if (_simulator.CurrentBody == null) return;

            float launchHeight = Mathf.Max(0f, _simulationSettings.InitialPosition.y);
            float projectileRadius = _simulator.CurrentBody.transform.localScale.y * 0.5f;
            float launchAngle = _simulationSettings.LaunchAngle;
            Vector3 basePosition = _simulator.CurrentState.Position;

            SetParameters(launchHeight, launchAngle, projectileRadius, basePosition);
        }

        public void UpdateExperimentStand(float height, float radius, float launchAngle)
        {
            Vector3 pos = new Vector3(0f, height, 0f);
            SetParameters(height, launchAngle, radius, pos);
        }

        public void SetParameters(float launchHeight, float launchAngle, float projectileRadius, Vector3 basePosition)
        {
            gameObject.SetActive(launchHeight > 0.05f);

            if (!gameObject.activeSelf)
                return;

            transform.position = new Vector3(0f, 0f, 0f);

            float poleHeight = launchHeight / 2f;
            poleHeight = Mathf.Max(0.01f, poleHeight - _baseHeight * 2f);

            _base.DOLocalMoveY(_baseHeight * 0.5f, _animationDuration);

            Vector3 scale = _pole.localScale;
            scale.y = poleHeight;
            _pole.DOScale(scale, _animationDuration);

            _pole.DOLocalMoveY(poleHeight + _baseHeight, _animationDuration);

            _platform.DOLocalMoveY(launchHeight - projectileRadius * 0.5f - _platformHeight * 2f, _animationDuration);
            _platform.DOLocalRotate(new Vector3(0f, 0f, Mathf.Clamp(launchAngle * 0.5f, 0f, 45f)), _animationDuration);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<ProjectileSpawnedSignal>(UpdateStand);
            _signalBus.TryUnsubscribe<SimulationSettingsChangedSignal>(UpdateStand);
        }
    }
}