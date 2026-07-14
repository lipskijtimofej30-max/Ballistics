using Assets.Game.Scripts.Core.Graphics;
using Game.Scripts.Core;
using Game.Scripts.Core.Simulation;
using Game.Scripts.UX;
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.Infrastructure.GameStateMachine.GameState
{
    public class SimulationFinishedState : IGameState
    {
        private readonly Simulator _simulator;
        private readonly SimulationPrinter _printer;
        private readonly DataExporter _exporter;
        private readonly SimulationAnalyzer _analyzer;
        private readonly ResultsPanelView _resultsPanel;
        private readonly ToolbarView _toolbarView;
        private readonly ParameterCanvasInteractable _parameterCanvasInteractable;
        private readonly GraphController _graphController;
        private readonly GraphView _graphView;
        
        private SimulationSummary _summary;

        [Inject]
        public SimulationFinishedState(
            Simulator simulator,
            SimulationPrinter printer,
            SimulationAnalyzer analyzer,
            DataExporter exporter,
            ResultsPanelView resultsPanel,
            ToolbarView toolbarView,
            ParameterCanvasInteractable parameterCanvasInteractable,
            GraphController graphController,
            GraphView graphView)
        {
            _simulator = simulator;
            _printer = printer;
            _analyzer = analyzer;
            _exporter = exporter;
            _resultsPanel = resultsPanel;
            _toolbarView = toolbarView;
            _parameterCanvasInteractable = parameterCanvasInteractable;
            _graphController = graphController;
            _graphView = graphView;
        }
        
        public void Enter()
        {
            _toolbarView.CreateButton.interactable = true;
            _toolbarView.PauseButton.interactable = false;
            _parameterCanvasInteractable.Toggle(true);
            
            var run = _simulator.CurrentRun;
            if (run == null) return;
            
            _printer.Print(run.Points);
            
            _summary = _analyzer.Analyze(run.Points);
            _resultsPanel.SetSummary(_summary);
            _resultsPanel.Show();
            _graphController.SetupSingleData(run);
            
            _resultsPanel.SaveCsvRequested += OnSaveCsvRequested;
        }

        public void Tick()
        {
            
        }

        public void Exit()
        {
            _resultsPanel.SaveCsvRequested -= OnSaveCsvRequested;
            _resultsPanel.Hide();
            _graphView.ClearAll();
            _graphController.ClearRun();
        }
        
        private void OnSaveCsvRequested()
        {
            var run = _simulator.CurrentRun;
            if (run == null) return;
            _exporter.ExportCsv(run.Points, _simulator.CurrentState, _summary);
        }
    }
}