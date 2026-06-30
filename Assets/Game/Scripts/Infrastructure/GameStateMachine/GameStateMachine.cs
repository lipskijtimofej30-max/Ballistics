using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Infrastructure.GameStateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<GameStateType, IGameState> _states = new();
        private readonly Dictionary<GameStateType, List<Transition>> _transitions = new();
    
        private IGameState _currentGameState;
        private GameStateType _currentGameStateId;
        
        public GameStateType CurrentGameStateType => _currentGameStateId;

        public void RegisterState(GameStateType id, IGameState gameState)
        {
            _states[id] = gameState;
            _transitions[id] = new List<Transition>(); 
        }

        public void AddTransition(GameStateType from, GameStateType to, Func<bool> condition)
        {
            _transitions[from].Add(new Transition(to, condition));
        }

        public void ChangeState(GameStateType id)
        {
            if (!_states.TryGetValue(id, out var nextState)) return;

            _currentGameState?.Exit();
            _currentGameState = nextState;
            _currentGameStateId = id;
            _currentGameState.Enter();
            
            Debug.LogWarning($"Changing state to {_currentGameStateId}");
        }

        public void Tick()
        {
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