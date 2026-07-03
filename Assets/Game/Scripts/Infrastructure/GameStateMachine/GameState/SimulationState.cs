using Game.Scripts.Core;
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.Infrastructure.GameStateMachine.GameState
{
    public class SimulationState : IGameState
    {
        private readonly Simulator _simulator;
        private readonly TelemetryPanelView _telemetryPanelView;

        [Inject]
        public SimulationState(Simulator simulator, TelemetryPanelView telemetryPanelView)
        {
            _simulator = simulator;
            _telemetryPanelView = telemetryPanelView;
        }
        
        public void Enter()
        {
            _telemetryPanelView.Show();
            if (!_simulator.HasActiveRun)
                _simulator.Begin();
            else
                _simulator.Resume();
        }

        public void Tick()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}