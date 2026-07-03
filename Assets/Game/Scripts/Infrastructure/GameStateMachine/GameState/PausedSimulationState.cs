using Game.Scripts.Core;
using Zenject;

namespace Game.Scripts.Infrastructure.GameStateMachine.GameState
{
    public class PausedSimulationState : IGameState
    {
        private readonly Simulator _simulator;

        [Inject]
        public PausedSimulationState(Simulator simulator)
        {
            _simulator = simulator;
        }
        
        public void Enter()
        {
            _simulator.Pause();    
        }

        public void Tick()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}