using Game.Scripts.Core;
using Game.Scripts.Settings;
using Game.Scripts.View.UseCase;
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.Infrastructure.GameStateMachine.GameState
{
    public class SetupSimulationState : IGameState
    {
        private readonly Simulator _simulator;
        private readonly SetupPanelView _setupPanelView;
        private readonly ToolbarView _toolbarView;
        private readonly VisualizationUseCase _visualizationUseCase;
        [Inject(Id ="Live")] private readonly TrajectoryRenderer _liveTrajectoryRenderer;

        [Inject]
        public SetupSimulationState(Simulator simulator, SetupPanelView setupPanelView, ToolbarView toolbarView, VisualizationUseCase visualizationUseCase)
        {
            _simulator = simulator;
            _setupPanelView = setupPanelView;
            _toolbarView = toolbarView;
            _visualizationUseCase = visualizationUseCase;
        }
        public void Enter()
        {
            _simulator.ClearProjectile();
            _setupPanelView.Show();
            _liveTrajectoryRenderer.SetVisible(false);
            _visualizationUseCase.SetPreviewAllowed(true);
            
            _toolbarView.CreateButton.interactable = true;
            _toolbarView.StartButton.interactable = false;
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