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
        private CsvExporter _csvExporter;
        private IPhysicsIntegrator _integrator;
        private ProjectileFactory _projectileFactory;
        private SignalBus _signalBus;
        
        private Projectile _currentProjectile;
        public SimulationRun CurrentRun { get; private set; }

        [field:SerializeField] public bool IsActive { get; private set; } = false;
        
        public bool HasActiveRun => CurrentRun != null;
        
        [Inject]
        private void Construct(IPhysicsIntegrator integrator, ProjectileFactory projectileFactory, 
             CsvExporter csvExporter, SignalBus signalBus)
        {
            _integrator = integrator;
            _projectileFactory = projectileFactory;
            _csvExporter = csvExporter;
            _signalBus = signalBus;
        }
        
        public void Spawn()
        {
            if (_currentProjectile != null)
                Destroy(_currentProjectile.gameObject);
            
            _currentProjectile = _projectileFactory.Create();
            CurrentRun = null;
            IsActive = false;
        }

        public void Begin()
        {
            if (_currentProjectile == null)
                Spawn();
            
            CurrentRun = new SimulationRun();
            IsActive = true;
        }
        
        public void ClearProjectile()
        {
            if(_currentProjectile != null)
                Destroy(_currentProjectile.gameObject);
            
            _currentProjectile = null;
            CurrentRun = null;
            IsActive = false;
        }
        
        public void Resume() => IsActive = true;
        public void Pause() => IsActive = false;
        
        private void FixedUpdate()
        {
            if (!IsActive || _currentProjectile == null || CurrentRun == null) return;
            
            _integrator.Step(_currentProjectile, CurrentRun, Time.fixedDeltaTime, OnProjectileLanded);
        }

        private void OnProjectileLanded()
        {
            IsActive = false;
            _signalBus.Fire(new ChangeStateSignal(GameStateType.FinishedSimulation));
        }
    }
}