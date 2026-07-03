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
        private SimulationPrinter _printer;
        private IPhysicsIntegrator _integrator;
        private ProjectileFactory _projectileFactory;
        private SignalBus _signalBus;
        
        private Projectile _currentProjectile;
        private SimulationRun _currentRun;

        [field:SerializeField] public bool IsActive { get; private set; } = false;
        
        public bool HasActiveRun => _currentRun != null;
        
        [Inject]
        private void Construct(IPhysicsIntegrator integrator, ProjectileFactory projectileFactory, 
            SimulationPrinter printer, CsvExporter csvExporter, SignalBus signalBus)
        {
            _integrator = integrator;
            _projectileFactory = projectileFactory;
            _printer = printer;
            _csvExporter = csvExporter;
            _signalBus = signalBus;
        }
        
        public void Spawn()
        {
            if (_currentProjectile != null)
                Destroy(_currentProjectile.gameObject);
            
            _currentProjectile = _projectileFactory.Create();
            _currentRun = null;
            IsActive = false;
        }

        public void Begin()
        {
            if (_currentProjectile == null)
                Spawn();
            
            _currentRun = new SimulationRun();
            IsActive = true;
        }
        
        public void ClearProjectile()
        {
            if(_currentProjectile != null)
                Destroy(_currentProjectile.gameObject);
            
            _currentProjectile = null;
            _currentRun = null;
            IsActive = false;
        }
        
        public void Resume() => IsActive = true;
        public void Pause() => IsActive = false;
        

        private void SaveCsv(SimulationRun run)
        {
            string path = Path.Combine(Application.persistentDataPath, "simulation.csv");
            _csvExporter.Export(path, run.Points);
        }
        
        private void FixedUpdate()
        {
            if (!IsActive || _currentProjectile == null || _currentRun == null) return;
            
            _integrator.Step(_currentProjectile, _currentRun, Time.fixedDeltaTime, OnProjectileLanded);
        }

        private void OnProjectileLanded()
        {
            IsActive = false;
            _signalBus.Fire(new ChangeStateSignal(GameStateType.FinishedSimulation));
        }
    }
}