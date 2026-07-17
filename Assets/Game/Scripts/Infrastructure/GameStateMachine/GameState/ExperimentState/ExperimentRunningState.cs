using Game.Scripts.Infrastructure.GameStateMachine;
using Assets.Game.Scripts.Core.Experiment;
using Assets.Game.Scripts.Core.Experiment.Parameter;
using Assets.Game.Scripts.Settings;
using Assets.Game.Scripts.View.View;
using Game.Scripts.Core;
using Game.Scripts.Settings;
using Game.Scripts.UX;
using Game.Scripts.View.View;
using UnityEngine;
using Zenject;
using ILogger = Game.Scripts.Infrastructure.Logger.ILogger;

namespace Assets.Game.Scripts.Infrastructure.GameStateMachine.ExperimentState
{
    public class ExperimentRunningState : IGameState
    {
        private readonly ExperimentParameterDataBase  _parameters;
        private readonly ExperimentPlaybackController _experimentController;
        private readonly ExperimentPlaybackSequencer _sequencer;
        private readonly ExperimentSession _session;
        private readonly ExperimentRunner _experimentRunner;
        private readonly ExperimentSettings  _experimentSettings;
        private readonly TrajectoryPool _pool;
        private readonly ToolbarView _toolbarView;
        private readonly TelemetryPanelView _telemetryPanelView;
        private readonly ResultsPanelView _resultsPanelView;
        private readonly ParameterCanvasInteractable _parameterCanvasInteractable;
        private readonly VectorRenderer _vectorRenderer;
        private readonly ILogger _logger;

        [Inject]
        public ExperimentRunningState(ExperimentParameterDataBase parameters, ExperimentPlaybackController experimentController,
            ExperimentPlaybackSequencer sequencer, ToolbarView toolbarView, ExperimentSession session, ExperimentRunner experimentRunner, 
            ExperimentSettings experimentSettings, TrajectoryPool pool, TelemetryPanelView telemetryPanelView, ResultsPanelView resultsPanelView,
            ParameterCanvasInteractable parameterCanvasInteractable, VectorRenderer vectorRenderer, ILogger logger)
        {
            _parameters = parameters;
            _experimentController = experimentController;
            _sequencer = sequencer;
            _session = session;
            _toolbarView = toolbarView;
            _experimentRunner = experimentRunner;
            _pool = pool;
            _experimentSettings = experimentSettings;
            _telemetryPanelView = telemetryPanelView;
            _resultsPanelView = resultsPanelView;
            _parameterCanvasInteractable = parameterCanvasInteractable;
            _vectorRenderer = vectorRenderer;
            _logger = logger;
        }
        
        public void Enter()
        {
            _toolbarView.LaboratoryButton.interactable = false;
            _vectorRenderer.ClearAll();
            _parameterCanvasInteractable.Toggle(false);
            _telemetryPanelView.Show();
            _resultsPanelView.Hide();
            
            _sequencer.RunPlaybackStarted += OnRunStarted;
            _sequencer.RunPlaybackFinished += OnRunFinished;
            
            if (_session.ExperimentRunResults.Count == 0)
            {
                _pool.ClearAll();
                var parameter = _parameters.GetCurrentParameter();
                var preset = new ExperimentPreset(ShapeType.Sphere, 5000f, 0.17f, 15f, 0f, 10f, 
                    new Vector3(0f, -9.81f, 0f), false, new Vector3(10f,0f,0f), IntegratorMethod.RK2, 0.01f);
                
                var results = _experimentRunner.RunSeries(
                    parameter, _experimentSettings.MinValue, _experimentSettings.MaxValue, _experimentSettings.Step, preset);
                
                _logger.Log($"Parameter {parameter.DisplayName} with min value {_experimentSettings.MinValue}, max value {_experimentSettings.MaxValue}," +
                            $" step {_experimentSettings.Step}, pause {_experimentSettings.PauseBetweenRuns}");
                
                _session.ClearAll();
                foreach (var result in results)
                    _session.Register(result);
                
                _sequencer.StartSequence();
            }
            else
            {
                _experimentController.Resume();
            }
        }
        
        public void Tick()
        {
           _experimentController.Tick(Time.deltaTime);
           if (_experimentController.IsPlaying)
           {
               int currentIndex = _experimentController.CurrentRunIndex;
               var currentPoint = _experimentController.CurrentPoint;
               
               _telemetryPanelView.SetExperimentPoint(currentIndex, currentPoint);

               float projectileSize = _session.ExperimentRunResults[0].Preset.Size;
               
               _vectorRenderer.UpdateVectors(
                   currentPoint.Position,
                   currentPoint.Velocity,
                   currentPoint.Acceleration,
                   currentPoint.TotalForce,
                   projectileSize);
           }
           else
           {
               _vectorRenderer.ClearAll();
           }
        }

        public void Exit()
        {
            _telemetryPanelView.Hide();
            _resultsPanelView.Hide();
            _toolbarView.LaboratoryButton.interactable = true;
            
            _sequencer.RunPlaybackStarted -= OnRunStarted;
            _sequencer.RunPlaybackFinished -= OnRunFinished;
        }

        private void OnRunStarted()
        {
            _resultsPanelView.Hide();
            _telemetryPanelView.Show();
        }

        private void OnRunFinished(ExperimentRunResult result)
        {
            _telemetryPanelView.Hide();
            _resultsPanelView.Show();
            _resultsPanelView.SetExperimentSummary(result, _parameters.GetCurrentParameter());
        }
    }
}
