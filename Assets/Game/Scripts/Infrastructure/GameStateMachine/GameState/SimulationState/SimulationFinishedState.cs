using System.Collections.Generic;
using Assets.Game.Scripts.Core.Graphics;
using Assets.Game.Scripts.View.View;
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
        private readonly VectorRenderer _vectorRenderer;
        
        private SimulationSummary _currentSummary;
        private SimulationSummary _previousSummary;

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
            GraphView graphView,
            VectorRenderer vectorRenderer)
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
            _vectorRenderer = vectorRenderer;
        }
        
        public void Enter()
        {
            _toolbarView.CreateButton.interactable = true;
            _toolbarView.PauseButton.interactable = false;
            _toolbarView.NewCreateButton.interactable = true;
            _parameterCanvasInteractable.Toggle(true);
            
            var previousRun = _simulator.PreviousRun;
            var currentRun = _simulator.CurrentRun;
            
            if(currentRun == null) return;
            
            _currentSummary = _analyzer.Analyze(currentRun.Points);
            if (previousRun != null)
            {
                _previousSummary = _analyzer.Analyze(previousRun.Points);
                _resultsPanel.SetSimulationComparisons(_analyzer.Compares(_previousSummary,  _currentSummary));
            }
            else
                _resultsPanel.SetSimulationSummary(_currentSummary);
            
            _resultsPanel.Show();
            _graphController.SetupSingleData(currentRun);
            
            _printer.Print(currentRun.Points);

            _resultsPanel.SaveCsvRequested += OnSaveCsvRequested;
        }

        public void Tick() { }

        public void Exit()
        {
            _resultsPanel.SaveCsvRequested -= OnSaveCsvRequested;
            _resultsPanel.Hide();
            _graphView.ClearAll();
            _graphController.ClearRun();
            _vectorRenderer.ClearAll();
        }
        
        private void OnSaveCsvRequested()
        {
            var run = _simulator.CurrentRun;
            if (run == null) return;
            _exporter.ExportCsv(run.Points, _simulator.CurrentState, _currentSummary);
        }
    }
}