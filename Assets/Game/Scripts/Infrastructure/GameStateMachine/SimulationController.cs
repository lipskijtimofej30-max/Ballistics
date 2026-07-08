using System;
using DefaultNamespace;
using Game.Scripts.Infrastructure.GameStateMachine.GameState;
using Game.Scripts.Infrastructure.Logger;
using Zenject;

namespace Game.Scripts.Infrastructure.GameStateMachine
{
    public class SimulationController : IDisposable
    {
        private GameStateMachine<SimulationStateType> _stateMachine;
        private SignalBus _signalBus;
        private ILogger _logger;

        [Inject]
        private void Construct(GameStateMachine<SimulationStateType> stateMachine, SignalBus signalBus, SetupSimulationState setupSimulationState,
            SimulationState simulationState, PausedSimulationState pausedSimulationState, FinishedSimulationState finishedSimulationState, ILogger logger)
        {
            _stateMachine = stateMachine;
            _signalBus = signalBus;
            _logger = logger;
            
            _stateMachine.RegisterState(SimulationStateType.SetupSimulation, setupSimulationState);
            _stateMachine.RegisterState(SimulationStateType.Simulation, simulationState);
            _stateMachine.RegisterState(SimulationStateType.PausedSimulation, pausedSimulationState);
            _stateMachine.RegisterState(SimulationStateType.FinishedSimulation, finishedSimulationState);
            
            _stateMachine.ChangeState(SimulationStateType.SetupSimulation);
            
            _signalBus.Subscribe<ChangeStateSignal<SimulationStateType>>(OnChangeState);
        }

        private void OnChangeState(ChangeStateSignal<SimulationStateType> signal)
        {
            _logger.LogWarning($"Changing state to {signal.NextState}");
            _stateMachine.ChangeState(signal.NextState);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ChangeStateSignal<SimulationStateType>>(OnChangeState);
        }
    }
}