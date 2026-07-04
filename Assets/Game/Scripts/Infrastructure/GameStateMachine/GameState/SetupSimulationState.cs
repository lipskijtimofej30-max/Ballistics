using Game.Scripts.Core;
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.Infrastructure.GameStateMachine.GameState
{
    public class SetupSimulationState : IGameState
    {
        private readonly Simulator _simulator;
        private readonly SetupPanelView _setupPanelView;
        private readonly ToolbarView _toolbarView;

        [Inject]
        public SetupSimulationState(Simulator simulator, SetupPanelView setupPanelView, ToolbarView toolbarView)
        {
            _simulator = simulator;
            _setupPanelView = setupPanelView;
            _toolbarView = toolbarView;
        }
        public void Enter()
        {
            _simulator.ClearProjectile();
            _setupPanelView.Show();
            
            _toolbarView.CreateButton.interactable = true;
            _toolbarView.StartButton.interactable = false;
        }

        public void Tick()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}