using System;

namespace Game.Scripts.Infrastructure.GameStateMachine
{
    public class Transition
    {
        public GameStateType TargetStateType { get; }
        public Func<bool> Condition { get; }

        public Transition(GameStateType targetStateType,  Func<bool> condition)
        {
            Condition = condition;
            TargetStateType = targetStateType;
        }
    }
}