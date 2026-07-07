using Game.Scripts.Core;
using Game.Scripts.Core.Simulation;
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.Infrastructure.GameStateMachine.GameState
{
    public class FinishedSimulationState : IGameState
    {
        private readonly Simulator _simulator;
        private readonly SimulationPrinter _printer;
        private readonly SimulationExporter _exporter;
        private readonly SimulationAnalyzer _analyzer;
        private readonly ResultsPanelView _resultsPanel;
        private readonly ToolbarView _toolbarView;

        [Inject]
        public FinishedSimulationState(
            Simulator simulator,
            SimulationPrinter printer,
            SimulationAnalyzer analyzer,
            SimulationExporter exporter,
            ResultsPanelView resultsPanel,
            ToolbarView toolbarView)
        {
            _simulator = simulator;
            _printer = printer;
            _analyzer = analyzer;
            _exporter = exporter;
            _resultsPanel = resultsPanel;
            _toolbarView = toolbarView;
        }
        
        public void Enter()
        {
            var run = _simulator.CurrentRun;
            if (run == null) return;
            
            //_printer.Print(run.Points);
            
            var summary = _analyzer.Analyze(run.Points);
            _resultsPanel.SetSummary(summary);
            _resultsPanel.Show();
            
            _toolbarView.CreateButton.interactable = true;
            _toolbarView.PauseButton.interactable = false;
            
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
            _exporter.ExportCsv(run.Points);
        }
    }
}