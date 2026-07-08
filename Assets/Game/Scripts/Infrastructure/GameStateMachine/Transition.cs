using System;

namespace Game.Scripts.Infrastructure.GameStateMachine
{
    public class Transition<T>
    {
        public T TargetStateType { get; }
        public Func<bool> Condition { get; }

        public Transition(T targetStateType,  Func<bool> condition)
        {
            Condition = condition;
            TargetStateType = targetStateType;
        }
    }
}