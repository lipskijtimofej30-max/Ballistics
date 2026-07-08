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
        private void Construct(GameStateMachine<SimulationStateType> stateMachine, SignalBus signalBus, SimulationSetupState simulationSetupState,
            SimulationState simulationState, SimulationPausedState simulationPausedState, SimulationFinishedState simulationFinishedState, ILogger logger)
        {
            _stateMachine = stateMachine;
            _signalBus = signalBus;
            _logger = logger;
            
            _stateMachine.RegisterState(SimulationStateType.SetupSimulation, simulationSetupState);
            _stateMachine.RegisterState(SimulationStateType.Simulation, simulationState);
            _stateMachine.RegisterState(SimulationStateType.PausedSimulation, simulationPausedState);
            _stateMachine.RegisterState(SimulationStateType.FinishedSimulation, simulationFinishedState);
            
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