using Game.Scripts.Infrastructure.GameStateMachine;
using System.Collections.Generic;
using Assets.Game.Scripts.Core.Experiment;
using Assets.Game.Scripts.Core.Experiment.Parameter;
using Assets.Game.Scripts.Settings;
using Game.Scripts.Core;
using Game.Scripts.Settings;
using Game.Scripts.View.View;
using UnityEngine;
using Zenject;
using ILogger = Game.Scripts.Infrastructure.Logger.ILogger;

namespace Assets.Game.Scripts.Infrastructure.GameStateMachine.ExperimentState
{
    public class ExperimentRunningState : IGameState
    {
        private readonly List<IExperimentParameter>  _parameters;
        private readonly ExperimentPlaybackController _experimentController;
        private readonly ExperimentPlaybackSequencer _sequencer;
        private readonly ExperimentSession _session;
        private readonly ExperimentRunner _experimentRunner;
        private readonly ExperimentSettings  _experimentSettings;
        private readonly IntegratorSettings _integratorSettings;
        private readonly TrajectoryPool _pool;
        private readonly ToolbarView _toolbarView;
        private readonly ILogger _logger;

        [Inject]
        public ExperimentRunningState(List<IExperimentParameter> parameters, ExperimentPlaybackController experimentController, ExperimentPlaybackSequencer sequencer, ToolbarView toolbarView,
            ExperimentSession session, ExperimentRunner experimentRunner, ExperimentSettings experimentSettings, IntegratorSettings integratorSettings, TrajectoryPool pool, ILogger logger)
        {
            _parameters = parameters;
            _experimentController = experimentController;
            _sequencer = sequencer;
            _session = session;
            _toolbarView = toolbarView;
            _experimentRunner = experimentRunner;
            _pool = pool;
            _experimentSettings = experimentSettings;
            _integratorSettings = integratorSettings;
            _logger = logger;
        }
        
        public void Enter()
        {
            _toolbarView.LaboratoryButton.interactable = false;
            if (_session.ExperimentRunResults.Count == 0)
            {
                _pool.ClearAll();
                var parameter = _parameters[_experimentSettings.SelectedParameterIndex];
                var preset = new ExperimentPreset(ShapeType.Sphere, 5000f, 0.17f, 15f, 0f, 10f, 
                    new Vector3(0f, -9.81f, 0f), false, new Vector3(10f,0f,0f), IntegratorMethod.RK2, 0.01f);
                var results = _experimentRunner.RunSeries(
                    parameter, _experimentSettings.MinValue, _experimentSettings.MaxValue, _experimentSettings.Step, preset);
                
                _logger.Log($"Parameter {parameter.DisplayName}");

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
        }

        public void Exit()
        {
            _toolbarView.LaboratoryButton.interactable = true;
        }
    }
}
