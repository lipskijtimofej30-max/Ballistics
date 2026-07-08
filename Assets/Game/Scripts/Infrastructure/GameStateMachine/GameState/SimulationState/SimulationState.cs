using Game.Scripts.Core;
using Game.Scripts.Core.Simulation;
using Game.Scripts.View.UseCase;
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.Infrastructure.GameStateMachine.GameState
{
    public class SimulationState : IGameState
    {
        private readonly Simulator _simulator;
        private readonly TelemetryPanelView _telemetryPanelView;
        private readonly ToolbarView _toolbarView;
        private readonly VisualizationUseCase _visualizationUseCase;
        [Inject(Id = "Live")] private readonly TrajectoryRenderer _trajectoryRenderer;

        [Inject]
        public SimulationState(Simulator simulator, TelemetryPanelView telemetryPanelView, ToolbarView toolbarView, VisualizationUseCase visualizationUseCase)
        {
            _simulator = simulator;
            _telemetryPanelView = telemetryPanelView;
            _toolbarView = toolbarView;
            _visualizationUseCase = visualizationUseCase;
        }
        
        public void Enter()
        {
            _toolbarView.CreateButton.interactable = false;
            _trajectoryRenderer.SetVisible(true);
            _visualizationUseCase.SetPreviewAllowed(false);
            
            if (!_simulator.HasActiveRun)
                _simulator.Begin();
            else
                _simulator.Resume();
            
            _telemetryPanelView.Show();
            _telemetryPanelView.SetPoint(new SimulationPoint());
        }

        public void Tick()
        {
            var run = _simulator.CurrentRun;
            if (run == null || run.Points.Count == 0) return;
            _telemetryPanelView.SetPoint(run.Points[^1]);
        }

        public void Exit()
        {
            _simulator.Pause();
            _telemetryPanelView.Hide();   
            _trajectoryRenderer.FlushBuffer();
        }
    }
}