using System.IO;
using DefaultNamespace;
using Game.Scripts.Core.Simulation;
using Game.Scripts.Infrastructure.GameStateMachine;
using Game.Scripts.Infrastructure.Signals;
using Game.Scripts.Settings;
using Game.Scripts.View.View;
using UnityEngine;
using Zenject;
using ILogger = Game.Scripts.Infrastructure.Logger.ILogger;

namespace Game.Scripts.Core
{
    public class Simulator : MonoBehaviour
    {
        private const int MaxStepsPerFrame = 200; // защита от "спирали смерти"
        
        private IPhysicsIntegrator _integrator;
        private IntegratorFactory _integratorFactory;
        private ProjectileFactory _projectileFactory;
        private IntegratorSettings _integratorSettings;
        private SignalBus _signalBus;
        private ILogger _logger;
        [Inject(Id = "Live")] private TrajectoryRenderer _trajectoryRenderer;
        
        public ProjectileBody CurrentBody { get; private set; }
        public ProjectileState CurrentState { get; private set; }
        public SimulationRun CurrentRun { get; private set; }

        [field:SerializeField] public bool IsActive { get; private set; } = false;
        
        public bool HasActiveRun => CurrentRun != null;
        
        private float _accumulator;

        [Inject]
        private void Construct(IntegratorFactory integratorFactory, ProjectileFactory projectileFactory,
            IntegratorSettings integratorSettings, SignalBus signalBus, ILogger logger)
        {
            _integratorFactory =  integratorFactory;
            _projectileFactory = projectileFactory;
            _integratorSettings = integratorSettings;
            _signalBus = signalBus;
            _logger = logger;
        }
        
        public void Spawn()
        {
            if (CurrentBody != null)
                Destroy(CurrentBody.gameObject);
            
            CurrentBody = _projectileFactory.CreateBody();
            CurrentState = _projectileFactory.CreateState();
            CurrentBody.SyncTransform(CurrentState.Position);
            CurrentRun = null;
            IsActive = false;
            _trajectoryRenderer.Clear();
            
            _signalBus.Fire(new ProjectileSpawnedSignal());
        }

        public void Begin()
        {
           _integrator = _integratorFactory.Create(_integratorSettings.IntegratorMethod);
            CurrentRun = new SimulationRun();
            CurrentRun.AddPoint(CurrentState.Position, CurrentState.Velocity, Vector3.zero, Vector3.zero, 0f);
            IsActive = true;
        }
        
        public void ClearProjectile()
        {
            if(CurrentBody != null)
                Destroy(CurrentBody.gameObject);
            
            CurrentBody = null;
            CurrentRun = null;
        }
        
        public void Resume() => IsActive = true;
        public void Pause() => IsActive = false;
        
        private void FixedUpdate()
        {
            if (!IsActive || CurrentBody == null || CurrentRun == null || CurrentState == null) return;

            float realDt = Time.fixedDeltaTime;
            float modelDt = realDt * _integratorSettings.TimeScale; // сколько модельного времени нужно смоделировать
            float physicsStep = _integratorSettings.IntegrationStep;
    
            bool landed = false;
            float remaining = modelDt;
            int steps = 0;
    
            while (remaining > 0 && steps < MaxStepsPerFrame)
            {
                float step = Mathf.Min(physicsStep, remaining);
                _integrator.Step(CurrentState, CurrentRun, step, () => landed = true);
                remaining -= step;
                steps++;
        
                if (landed)
                {
                    remaining = 0;
                }
            }
            
            if(steps > 2)
                _logger.Log($"Steps {steps}");
            
            CurrentBody.SyncTransform(CurrentState.Position);
            _trajectoryRenderer.AppendPoint(CurrentState.Position);
            
            if (landed)
                OnProjectileLanded();
        }

        private void OnProjectileLanded()
        {
            _signalBus.Fire(new ChangeStateSignal(GameStateType.FinishedSimulation));
        }
    }
}