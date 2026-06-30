namespace Game.Scripts.Infrastructure.GameStateMachine
{
    public interface IGameState
    {
        void Enter();
        void Tick();
        void Exit();
    }
}