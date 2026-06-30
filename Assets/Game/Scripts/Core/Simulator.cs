using System;
using System.IO;
using Game.Scripts.Core.Simulation;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class Simulator : MonoBehaviour
    {
        private SimulationRecorder _recorder;
        private CsvExporter _csvExporter;
        private SimulationPrinter _printer;
        private IPhysicsIntegrator _integrator;
        private ProjectileFactory _projectileFactory;
        
        private Projectile _currentProjectile;

        [field:SerializeField] public bool IsActive { get; private set; } = false;
        
        [Inject]
        private void Construct(IPhysicsIntegrator integrator, ProjectileFactory projectileFactory,
            SimulationRecorder recorder, SimulationPrinter printer, CsvExporter csvExporter)
        {
            _integrator = integrator;
            _projectileFactory = projectileFactory;
            _recorder = recorder;
            _printer = printer;
            _csvExporter = csvExporter;
        }
        
        [ContextMenu("Spawn")]
        public void Spawn()
        {
            _currentProjectile = _projectileFactory.Create();
            StartSimulation();
        }
        [ContextMenu("Start Simulation")]
        public void StartSimulation()
        {
            _recorder.Clear();
            IsActive = true;
        }

        [ContextMenu("Stop Simulation")]
        public void StopSimulation()
        {
            IsActive = false;
            _printer.Print(_recorder.Points);
            SaveCsv();
        }
        
        [ContextMenu("Save CSV")]
        public void SaveCsv()
        {
            string path =
                Path.Combine(Application.persistentDataPath, "simulation.csv");

            _csvExporter.Export(path, _recorder.Points);
        }
        
        private void FixedUpdate()
        {
            if (!IsActive) return;
            if(_currentProjectile == null) return;
            
            _integrator.Step(_currentProjectile, StopSimulation);
        }
    }
}