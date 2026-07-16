using Game.Scripts.Infrastructure.GameStateMachine;
using Assets.Game.Scripts.Core.Experiment;
using Assets.Game.Scripts.Core.Experiment.Parameter;
using Game.Scripts.Core;
using Game.Scripts.Core.Simulation;
using Game.Scripts.UX;
using Game.Scripts.View.View;
using Zenject;
using Assets.Game.Scripts.Core.Graphics;
using System.Collections.Generic;
using System;
using Assets.Game.Scripts.View.View;
using Game.Scripts.Infrastructure.Logger;

namespace Assets.Game.Scripts.Infrastructure.GameStateMachine.ExperimentState
{
    public class ExperimentFinishedState : IGameState
    {
        private readonly ExperimentPlaybackSequencer _sequencer;
        private readonly ExperimentSession _session;
        private readonly ExperimentParameterDataBase _parameters;
        private readonly ExperimentTableView _tableView;
        private readonly DataExporter _exporter;
        private readonly TrajectoryPool _pool;
        private readonly GraphController _graphController;
        private readonly ExperimentGraphFilterController _graphFilterController;
        private readonly GraphView _graphView;
        private readonly GraphLegendView _legendView;
        private readonly ParameterCanvasInteractable _parameterCanvasInteractable;
        private readonly VectorRenderer _vectorRenderer;
        private readonly ILogger _logger;

        [Inject]
        public ExperimentFinishedState(ExperimentPlaybackSequencer sequencer, ExperimentSession session,
            TrajectoryPool pool, ExperimentTableView tableView, ExperimentParameterDataBase parameters,
            DataExporter exporter, GraphController graphController,
            ParameterCanvasInteractable parameterCanvasInteractable,
            ILogger logger, GraphView graphGraphView, ExperimentGraphFilterController graphFilterController,
            VectorRenderer vectorRenderer, GraphLegendView legendView)
        {
            _sequencer = sequencer;
            _session = session;
            _pool = pool;
            _tableView = tableView;
            _parameters = parameters;
            _exporter = exporter;
            _graphController = graphController;
            _parameterCanvasInteractable = parameterCanvasInteractable;
            _logger = logger;
            _graphFilterController = graphFilterController;
            _graphView = graphGraphView;
            _vectorRenderer = vectorRenderer;
            _legendView = legendView;
        }
        public void Enter()
        {
            try
            {
                List<SimulationRun> runs = new();
                foreach (var result in _session.ExperimentRunResults)
                    runs.Add(result.Run);

                _graphFilterController.Initialize();
                _legendView.Show();
            }
            catch(Exception e)
            {
                _logger.LogError($"[ExperimentFinishedState] {e.ToString()} {e.StackTrace}");
            }
            
            _parameterCanvasInteractable.Toggle(true);
            _tableView.SaveCsvRequested += OnSaveCsvRequested;
            _tableView.Show(_parameters.GetCurrentParameter(), _session.ExperimentRunResults);
        }
        
        public void Tick()
        {
        }

        public void Exit()
        {
            _tableView.SaveCsvRequested -= OnSaveCsvRequested;
            _graphFilterController.Dispose();
            _graphController.ClearRuns();
            _tableView.Hide();
            _sequencer.StopSequence();
            _pool.ClearAll();
            _session.ClearAll();
            _graphView.ClearAll();
            _vectorRenderer.ClearAll();
            _legendView.Hide();
        }

        private void OnSaveCsvRequested()
        {
            if(_session.ExperimentRunResults.Count == 0) return;
            
            var preset = _session.ExperimentRunResults[0].Preset;
            _exporter.ExportTableExperimentCsv(_session.ExperimentRunResults, _parameters.GetCurrentParameter(), preset);
        }
    }
}
