using System;
using DefaultNamespace;
using Game.Scripts.Infrastructure.GameStateMachine.GameState;
using Zenject;

namespace Game.Scripts.Infrastructure.GameStateMachine
{
    public class GameController : ITickable, IDisposable
    {
        private GameStateMachine _stateMachine;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(GameStateMachine stateMachine, SignalBus signalBus, SetupSimulationState setupSimulationState,
            SimulationState simulationState, PausedSimulationState pausedSimulationState, FinishedSimulationState finishedSimulationState)
        {
            _stateMachine = stateMachine;
            _signalBus = signalBus;
            
            _stateMachine.RegisterState(GameStateType.SetupSimulation, setupSimulationState);
            _stateMachine.RegisterState(GameStateType.Simulation, simulationState);
            _stateMachine.RegisterState(GameStateType.PausedSimulation, pausedSimulationState);
            _stateMachine.RegisterState(GameStateType.FinishedSimulation, finishedSimulationState);
            
            _stateMachine.ChangeState(GameStateType.SetupSimulation);
            
            _signalBus.Subscribe<ChangeStateSignal>(OnChangeState);
        }

        public void Tick()
        {
            _stateMachine.Tick();
        }

        private void OnChangeState(ChangeStateSignal signal)
        {
            _stateMachine.ChangeState(signal.NextState);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ChangeStateSignal>(OnChangeState);
        }
    }
}