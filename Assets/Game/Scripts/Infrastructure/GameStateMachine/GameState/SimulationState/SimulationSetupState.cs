using Game.Scripts.Core;
using Game.Scripts.Settings;
using Game.Scripts.View.UseCase;
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.Infrastructure.GameStateMachine.GameState
{
    public class SimulationSetupState : IGameState
    {
        private readonly Simulator _simulator;
        private readonly SetupPanelView _setupPanelView;
        private readonly ToolbarView _toolbarView;
        private readonly VisualizationUseCase _visualizationUseCase;
        private readonly ModeControllerView _modeControllerView;
        [Inject(Id ="Live")] private readonly TrajectoryRenderer _liveTrajectoryRenderer;

        [Inject]
        public SimulationSetupState(Simulator simulator, SetupPanelView setupPanelView, ToolbarView toolbarView, VisualizationUseCase visualizationUseCase, ModeControllerView modeControllerView)
        {
            _simulator = simulator;
            _setupPanelView = setupPanelView;
            _toolbarView = toolbarView;
            _visualizationUseCase = visualizationUseCase;
            _modeControllerView = modeControllerView;
        }
        public void Enter()
        {
            _modeControllerView.HideObjectsForExperiment();
            _modeControllerView.ShowObjectsForSimulation();
            
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