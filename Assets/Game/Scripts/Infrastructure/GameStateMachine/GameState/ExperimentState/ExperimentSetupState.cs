using Game.Scripts.Infrastructure.GameStateMachine;
using System;
using Assets.Game.Scripts.Core.Experiment;
using Game.Scripts.Core;
using Game.Scripts.View.View;
using Zenject;

namespace Assets.Game.Scripts.Infrastructure.GameStateMachine.ExperimentState
{
    public class ExperimentSetupState : IGameState
    {
        private readonly ModeControllerView _modeControllerView;
        private readonly ToolbarView _toolbarView;
        private readonly TrajectoryRenderer _trajectoryRenderer;
        private readonly ExperimentPlaybackSequencer _sequencer;
        private readonly TrajectoryPool _pool;
        private readonly ExperimentSession _session;

        [Inject]
        public ExperimentSetupState(ModeControllerView modeControllerView, ToolbarView toolbarView,
            [Inject(Id = "Preview")] TrajectoryRenderer trajectoryRenderer, ExperimentPlaybackSequencer sequencer, TrajectoryPool pool, ExperimentSession session)
        {
            _modeControllerView = modeControllerView;
            _toolbarView = toolbarView;
            _trajectoryRenderer = trajectoryRenderer;
            _sequencer = sequencer;
            _pool = pool;
            _session = session;
        }
        public void Enter()
        {
            _sequencer.StopSequence();
            _pool.ClearAll();
            _session.ClearAll();
            _trajectoryRenderer.Clear();
            
            _modeControllerView.HideObjectsForSimulation();
            _modeControllerView.ShowObjectsForExperiment();
            
            _toolbarView.CreateButton.interactable = false;
            _toolbarView.StartButton.interactable = true;
            _toolbarView.PauseButton.interactable = false;
            _toolbarView.StopButton.interactable = false;
        }

        
        public void Tick()
        {
            
        }
        
        public void Exit()
        {
            _toolbarView.PauseButton.interactable = true;
            _toolbarView.StopButton.interactable = true;
        }
    }
}
