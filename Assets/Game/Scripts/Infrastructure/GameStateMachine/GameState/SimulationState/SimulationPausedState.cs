using Game.Scripts.Core;
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.Infrastructure.GameStateMachine.GameState
{
    public class SimulationPausedState : IGameState
    {
        private readonly Simulator _simulator;
        private readonly TelemetryPanelView _telemetryPanelView;

        [Inject]
        public SimulationPausedState(TelemetryPanelView telemetryPanelView, Simulator simulator)
        {
            _telemetryPanelView = telemetryPanelView;
            _simulator = simulator;
        }
        public void Enter()
        {
            _telemetryPanelView.SetPointForSimulationPause(_simulator.CurrentRun.Points[^1]);
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