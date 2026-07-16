using Game.Scripts.Infrastructure.GameStateMachine;
using Assets.Game.Scripts.Core.Experiment;
using Game.Scripts.View.View;
using Zenject;

namespace Assets.Game.Scripts.Infrastructure.GameStateMachine.ExperimentState
{
    public class ExperimentPauseState : IGameState
    {
        private readonly ExperimentPlaybackController _playbackController;
        private readonly TelemetryPanelView _telemetryPanelView;

        [Inject]
        public ExperimentPauseState(ExperimentPlaybackController playbackController, TelemetryPanelView telemetryPanelView)
        {
            _playbackController = playbackController;
            _telemetryPanelView = telemetryPanelView;
        }
        
        public void Enter()
        {
            _telemetryPanelView.SetPointForExperimentPause(_playbackController.CurrentRunIndex, _playbackController.CurrentPoint);
            _telemetryPanelView.Show();
            _playbackController.Pause();
        }
        
        public void Tick()
        {
            
        }

        public void Exit()
        {
            _telemetryPanelView.Hide();
        }
    }
}
