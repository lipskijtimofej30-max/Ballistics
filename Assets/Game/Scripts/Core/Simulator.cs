using System;
using System.IO;
using Game.Scripts.Core.Simulation;
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
        
        private Projectile _currentProjectile;
        private SimulationRun _currentRun;

        [field:SerializeField] public bool IsActive { get; private set; } = false;
        
        [Inject]
        private void Construct(IPhysicsIntegrator integrator, ProjectileFactory projectileFactory, 
            SimulationPrinter printer, CsvExporter csvExporter)
        {
            _integrator = integrator;
            _projectileFactory = projectileFactory;
            _printer = printer;
            _csvExporter = csvExporter;
        }
        
        [ContextMenu("Spawn and Start")]
        public void SpawnAndStart()
        {
            _currentProjectile = _projectileFactory.Create();
            _currentRun = new SimulationRun();
            IsActive = true;
        }

        [ContextMenu("Stop Simulation")]
        public void StopSimulation()
        {
            IsActive = false;
            if (_currentRun != null)
            {
                _printer.Print(_currentRun.Points);
                SaveCsv(_currentRun);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnAndStart();
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                StopSimulation();
            }
        }

        private void SaveCsv(SimulationRun run)
        {
            string path = Path.Combine(Application.persistentDataPath, "simulation.csv");
            _csvExporter.Export(path, run.Points);
        }
        
        private void FixedUpdate()
        {
            if (!IsActive || _currentProjectile == null || _currentRun == null) return;
            
            _integrator.Step(_currentProjectile, _currentRun, Time.fixedDeltaTime, StopSimulation);
        }
    }
}