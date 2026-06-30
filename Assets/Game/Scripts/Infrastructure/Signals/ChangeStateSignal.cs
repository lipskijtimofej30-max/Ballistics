using Game.Scripts.Infrastructure.GameStateMachine;

namespace DefaultNamespace
{
    public class ChangeStateSignal
    {
        public GameStateType NextState;

        public ChangeStateSignal(GameStateType nextState)
        {
            NextState = nextState;
        }
    }
}