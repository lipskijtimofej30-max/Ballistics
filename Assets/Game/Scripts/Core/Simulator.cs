using System.IO;
using DefaultNamespace;
using Game.Scripts.Core.Simulation;
using Game.Scripts.Infrastructure.GameStateMachine;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class Simulator : MonoBehaviour
    {
        private IPhysicsIntegrator _integrator;
        private ProjectileFactory _projectileFactory;
        private SignalBus _signalBus;
        
        public Projectile CurrentProjectile { get; private set; }
        public SimulationRun CurrentRun { get; private set; }

        [field:SerializeField] public bool IsActive { get; private set; } = false;
        
        public bool HasActiveRun => CurrentRun != null;
        
        [Inject]
        private void Construct(IPhysicsIntegrator integrator, ProjectileFactory projectileFactory, 
             SignalBus signalBus)
        {
            _integrator = integrator;
            _projectileFactory = projectileFactory;
            _signalBus = signalBus;
        }
        
        public void Spawn()
        {
            if (CurrentProjectile != null)
                Destroy(CurrentProjectile.gameObject);
            
            CurrentProjectile = _projectileFactory.Create();
            CurrentRun = null;
            IsActive = false;
        }

        public void Begin()
        {
            CurrentRun = new SimulationRun();
            IsActive = true;
        }
        
        public void ClearProjectile()
        {
            if(CurrentProjectile != null)
                Destroy(CurrentProjectile.gameObject);
            
            CurrentProjectile = null;
            CurrentRun = null;
        }
        
        public void Resume() => IsActive = true;
        public void Pause() => IsActive = false;
        
        private void FixedUpdate()
        {
            if (!IsActive || CurrentProjectile == null || CurrentRun == null) return;
            
            _integrator.Step(CurrentProjectile, CurrentRun, Time.fixedDeltaTime, OnProjectileLanded);
        }

        private void OnProjectileLanded()
        {
            _signalBus.Fire(new ChangeStateSignal(GameStateType.FinishedSimulation));
        }
    }
}