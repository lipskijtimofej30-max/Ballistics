using System.IO;
using DefaultNamespace;
using Game.Scripts.Core.Simulation;
using Game.Scripts.Infrastructure.GameStateMachine;
using Game.Scripts.Infrastructure.Signals;
using Game.Scripts.View.View;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class Simulator : MonoBehaviour
    {
        private IPhysicsIntegrator _integrator;
        private IntegratorFactory _integratorFactory;
        private ProjectileFactory _projectileFactory;
        private SignalBus _signalBus;
        [Inject(Id = "Live")] private TrajectoryRenderer _trajectoryRenderer;
        
        public ProjectileBody CurrentBody { get; private set; }
        public ProjectileState CurrentState { get; private set; }
        public SimulationRun CurrentRun { get; private set; }

        [field:SerializeField] public bool IsActive { get; private set; } = false;
        
        public bool HasActiveRun => CurrentRun != null;

        [Inject]
        private void Construct(IntegratorFactory integratorFactory, ProjectileFactory projectileFactory, SignalBus signalBus)
        {
            _integratorFactory =  integratorFactory;
            _projectileFactory = projectileFactory;
            _signalBus = signalBus;
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
           _integrator = _integratorFactory.Create(IntegratorMethod.SemiImplicitEuler);
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
            bool landed = false;
            
            _integrator.Step(CurrentState, CurrentRun, Time.fixedDeltaTime, () => landed = true);
            
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