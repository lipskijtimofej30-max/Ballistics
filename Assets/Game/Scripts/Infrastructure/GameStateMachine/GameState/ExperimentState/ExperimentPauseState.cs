using Game.Scripts.Infrastructure.GameStateMachine;
using System;
using Assets.Game.Scripts.Core.Experiment;
using Zenject;

namespace Assets.Game.Scripts.Infrastructure.GameStateMachine.ExperimentState
{
    public class ExperimentPauseState : IGameState
    {
        private readonly ExperimentPlaybackController _playbackController;

        [Inject]
        public ExperimentPauseState(ExperimentPlaybackController playbackController)
        {
            _playbackController = playbackController;
        }
        
        public void Enter()
        {
            _playbackController.Pause();
        }

        public void Exit()
        {
        }

        public void Tick()
        {
        }
    }
}
