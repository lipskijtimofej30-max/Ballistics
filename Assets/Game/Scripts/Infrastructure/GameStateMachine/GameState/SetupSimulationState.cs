using Game.Scripts.Core;
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.Infrastructure.GameStateMachine.GameState
{
    public class SetupSimulationState : IGameState
    {
        private readonly Simulator _simulator;
        private readonly SetupPanelView _setupPanelView;

        [Inject]
        public SetupSimulationState(Simulator simulator, SetupPanelView setupPanelView)
        {
            _simulator = simulator;
            _setupPanelView = setupPanelView;
        }
        public void Enter()
        {
            _simulator.ClearProjectile();
            _setupPanelView.Show();
        }

        public void Tick()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}