using Assets.Game.Scripts.View.View;
using Game.Scripts.Core;
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
        private readonly VectorRenderer _vectorRenderer;

        [Inject]
        public SimulationSetupState(Simulator simulator, SetupPanelView setupPanelView, ToolbarView toolbarView, 
            VisualizationUseCase visualizationUseCase, ModeControllerView modeControllerView, VectorRenderer vectorRenderer)
        {
            _simulator = simulator;
            _setupPanelView = setupPanelView;
            _toolbarView = toolbarView;
            _visualizationUseCase = visualizationUseCase;
            _modeControllerView = modeControllerView;
            _vectorRenderer = vectorRenderer;
        }
        public void Enter()
        {
            _modeControllerView.HideObjectsForExperiment();
            _modeControllerView.ShowObjectsForSimulation();
            
            _vectorRenderer.ClearAll();
            _setupPanelView.Show();
            _visualizationUseCase.SetPreviewAllowed(true);
            
            _toolbarView.CreateButton.interactable = true;
            _toolbarView.StartButton.interactable = false;
            _toolbarView.PauseButton.interactable = false;
            _toolbarView.StopButton.interactable = false;
            _toolbarView.NewCreateButton.interactable = false;
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