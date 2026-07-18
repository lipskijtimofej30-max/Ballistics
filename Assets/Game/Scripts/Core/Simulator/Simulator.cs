using DefaultNamespace;
using Game.Scripts.Core.Simulation;
using Game.Scripts.Infrastructure.GameStateMachine;
using Game.Scripts.Settings;
using UnityEngine;
using Zenject;
using ILogger = Game.Scripts.Infrastructure.Logger.ILogger;

namespace Game.Scripts.Core
{
    public class Simulator : MonoBehaviour
    {
        private const int MaxStepsPerFrame = 200;

        private IPhysicsIntegrator _integrator;
        private IntegratorFactory _integratorFactory;
        private IntegratorSettings _integratorSettings;
        private SignalBus _signalBus;
        private ILogger _logger;
        private SimulationStepper _stepper;

        private ProjectileManager _projectileManager;
        private SimulationVisualizer _visualizer;

        public bool IsActive { get; private set; }
        public SimulationRun CurrentRun { get; private set; }
        public SimulationRun PreviousRun { get; private set; }

        public ProjectileBody CurrentBody => _projectileManager.CurrentBody;
        public ProjectileState CurrentState => _projectileManager.CurrentState;
        public ProjectileState PreviousState { get; private set; }
        public bool HasActiveRun => CurrentRun != null;

        [Inject]
        private void Construct(
            IntegratorFactory integratorFactory,
            IntegratorSettings integratorSettings,
            SignalBus signalBus,
            ILogger logger,
            ProjectileManager projectileManager,
            SimulationVisualizer visualizer)
        {
            _integratorFactory = integratorFactory;
            _integratorSettings = integratorSettings;
            _signalBus = signalBus;
            _logger = logger;
            _projectileManager = projectileManager;
            _visualizer = visualizer;
        }

        public void Spawn(bool keepPrevious = false)
        {
            if (keepPrevious)
            {
                PreviousRun = CurrentRun;
                PreviousState = _projectileManager.CurrentState;
            }
            else
            {
                PreviousRun = null;
                PreviousState = null;
            }
            
            _projectileManager.Spawn(keepPrevious);
            CurrentRun = null;
            IsActive = false;
        }

        public void Begin()
        {
            _integrator = _integratorFactory.Create(_integratorSettings.IntegratorMethod);
            _stepper = new SimulationStepper(_integrator);
            CurrentRun = new SimulationRun();
            CurrentRun.AddPoint(CurrentState.Position, CurrentState.Velocity, Vector3.zero, Vector3.zero, 0f);
            IsActive = true;
        }

        public void ClearProjectile()
        {
            _projectileManager.ClearAll();
            _visualizer.ClearAll();
            CurrentRun = null;
        }

        public void Resume() => IsActive = true;
        public void Pause() => IsActive = false;

        private void FixedUpdate()
        {
            if (!IsActive || CurrentBody == null || CurrentRun == null || CurrentState == null) return;

            float realDt = Time.fixedDeltaTime;
            float modelDt = realDt * _integratorSettings.TimeScale;
            float physicsStep = _integratorSettings.IntegrationStep;

            bool landed = false;
            float remaining = modelDt;
            int steps = 0;

            while (remaining > 0 && steps < MaxStepsPerFrame)
            {
                float step = Mathf.Min(physicsStep, remaining);
                landed = _stepper.Step(CurrentState, CurrentRun, step);
                remaining -= step;
                steps++;

                if (landed)
                    break;
            }

            if (steps > 10)
                _logger.Log($"Steps {steps}");

            CurrentBody.SyncTransform(CurrentState.Position);
            _visualizer.UpdateVisuals(CurrentState);

            if (landed)
                OnProjectileLanded();
        }

        private void OnProjectileLanded()
        {
            if (_projectileManager.CurrentBody == null) return;
            IsActive = false;
            _visualizer.FlushCurrentTrajectory();
            _signalBus.Fire(new ChangeStateSignal<SimulationStateType>(SimulationStateType.FinishedSimulation));
        }
    }
}