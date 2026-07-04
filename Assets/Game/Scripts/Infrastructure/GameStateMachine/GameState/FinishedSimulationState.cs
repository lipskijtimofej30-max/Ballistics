using System.IO;
using Game.Scripts.Core;
using Game.Scripts.Core.Simulation;
using Game.Scripts.View.View;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Infrastructure.GameStateMachine.GameState
{
    public class FinishedSimulationState : IGameState
    {
        private readonly Simulator _simulator;
        private readonly SimulationPrinter _printer;
        private readonly CsvExporter _csvExporter;
        private readonly SimulationAnalyzer _analyzer;
        private readonly ResultsPanelView _resultsPanel;

        [Inject]
        public FinishedSimulationState(
            Simulator simulator,
            SimulationPrinter printer,
            CsvExporter csvExporter,
            SimulationAnalyzer analyzer,
            ResultsPanelView resultsPanel)
        {
            _simulator = simulator;
            _printer = printer;
            _csvExporter = csvExporter;
            _analyzer = analyzer;
            _resultsPanel = resultsPanel;
        }
        
        public void Enter()
        {
            var run = _simulator.CurrentRun;
            if (run == null) return;
            
            _printer.Print(run.Points);
            
            var summary = _analyzer.Analyze(run.Points);
            _resultsPanel.SetSummary(summary);
            _resultsPanel.Show();
            
            _resultsPanel.SaveCsvRequested += OnSaveCsvRequested;
        }

        public void Tick()
        {
            
        }

        public void Exit()
        {
            _resultsPanel.SaveCsvRequested -= OnSaveCsvRequested;
            _resultsPanel.Hide();
        }
        
        private void OnSaveCsvRequested()
        {
            var run = _simulator.CurrentRun;
            if (run == null) return;

            string path = Path.Combine(Application.persistentDataPath, "simulation.csv");
            _csvExporter.Export(path, run.Points);
        }
    }
}