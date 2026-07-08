
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.Infrastructure.GameStateMachine.GameState
{
    public class SimulationPausedState : IGameState
    {
        private readonly TelemetryPanelView _telemetryPanelView;

        [Inject]
        public SimulationPausedState(TelemetryPanelView telemetryPanelView)
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