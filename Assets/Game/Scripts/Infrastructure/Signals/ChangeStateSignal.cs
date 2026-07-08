using Game.Scripts.Infrastructure.GameStateMachine;

namespace DefaultNamespace
{
    public class ChangeStateSignal<TState>
    {
        public TState NextState;

        public ChangeStateSignal(TState nextState)
        {
            NextState = nextState;
        }
    }
}