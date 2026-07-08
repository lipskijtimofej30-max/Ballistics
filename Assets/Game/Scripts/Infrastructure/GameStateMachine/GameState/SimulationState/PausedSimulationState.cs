
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.Infrastructure.GameStateMachine.GameState
{
    public class PausedSimulationState : IGameState
    {
        private readonly TelemetryPanelView _telemetryPanelView;

        [Inject]
        public PausedSimulationState(TelemetryPanelView telemetryPanelView)
        {
            _telemetryPanelView = telemetryPanelView;
        }
        public void Enter()
        {
            _telemetryPanelView.Show();
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