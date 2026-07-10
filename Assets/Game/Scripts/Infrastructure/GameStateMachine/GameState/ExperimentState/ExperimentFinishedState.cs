using Game.Scripts.Infrastructure.GameStateMachine;
using Assets.Game.Scripts.Core.Experiment;
using Assets.Game.Scripts.Core.Experiment.Parameter;
using Game.Scripts.Core;
using Game.Scripts.Core.Simulation;
using Game.Scripts.UX;
using Game.Scripts.View.View;
using Zenject;

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
        private readonly ParameterCanvasInteractable _parameterCanvasInteractable;

        [Inject]
        public ExperimentFinishedState(ExperimentPlaybackSequencer sequencer, ExperimentSession session,
            TrajectoryPool pool,ExperimentTableView tableView, ExperimentParameterDataBase parameters, DataExporter exporter, ParameterCanvasInteractable parameterCanvasInteractable)
        {
            _sequencer = sequencer;
            _session = session;
            _pool = pool;
            _tableView = tableView;
            _parameters = parameters;
            _exporter = exporter;
            _parameterCanvasInteractable = parameterCanvasInteractable;
        }
        public void Enter()
        {
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
            _tableView.Hide();
            _sequencer.StopSequence();
            _pool.ClearAll();
            _session.ClearAll();
        }

        private void OnSaveCsvRequested()
        {
            if(_session.ExperimentRunResults.Count == 0) return;
            
            var preset = _session.ExperimentRunResults[0].Preset;
            _exporter.ExportTableExperimentCsv(_session.ExperimentRunResults, _parameters.GetCurrentParameter(), preset);
        }
    }
}
