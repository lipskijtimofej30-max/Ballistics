using System;
using System.Collections.Generic;

namespace Game.Scripts.Infrastructure.GameStateMachine
{
    public class GameStateMachine<T>
    {
        private readonly Dictionary<T, IGameState> _states = new();
        private readonly Dictionary<T, List<Transition<T>>> _transitions = new();

        private IGameState _currentGameState;
        private T _currentGameStateId;
        private bool _hasStarted = false;
        
        public void RegisterState(T id, IGameState gameState)
        {
            _states[id] = gameState;
            _transitions[id] = new List<Transition<T>>(); 
        }

        public void AddTransition(T from, T to, Func<bool> condition)
        {
            _transitions[from].Add(new Transition<T>(to, condition));
        }

        public void ChangeState(T id)
        {
            if (!_states.TryGetValue(id, out var nextState)) return;

            _currentGameState?.Exit();
            _currentGameState = nextState;
            _currentGameStateId = id;
            _hasStarted = true;
            _currentGameState.Enter();    
        }

        public void Tick()
        {
            if (!_hasStarted) return;

            if (_transitions.TryGetValue(_currentGameStateId, out var currentTransitions))
            {
                foreach (var transition in currentTransitions)
                {
                    if (transition.Condition())
                    {
                        ChangeState(transition.TargetStateType);
                        return; // Выходим, так как состояние изменилось
                    }
                }
            }

            _currentGameState?.Tick();
        }
    }
}