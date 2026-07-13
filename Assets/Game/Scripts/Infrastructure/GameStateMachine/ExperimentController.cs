using Assets.Game.Scripts.Infrastructure.GameStateMachine.ExperimentState;
using DefaultNamespace;
using Game.Scripts.Infrastructure.GameStateMachine;
using Game.Scripts.Infrastructure.Logger;
using System;
using Zenject;

namespace Assets.Game.Scripts.Infrastructure.GameStateMachine
{
    public class ExperimentController : IDisposable
    {
        private readonly GameStateMachine<ExperimentStateType> _stateMachine;
        private readonly SignalBus _signalBus;
        private readonly ILogger _logger;

        [Inject]
        public ExperimentController(GameStateMachine<ExperimentStateType> stateMachine, SignalBus signalBus, 
            ExperimentSetupState setupState, ExperimentRunningState runningState, ExperimentPauseState pauseState, ExperimentFinishedState finishedState,ILogger logger)
        {
            _stateMachine = stateMachine;
            _signalBus = signalBus;
            _logger = logger;

            _stateMachine.RegisterState(ExperimentStateType.Setup, setupState);
            _stateMachine.RegisterState(ExperimentStateType.Running, runningState);
            _stateMachine.RegisterState(ExperimentStateType.Pause, pauseState);
            _stateMachine.RegisterState(ExperimentStateType.Finished, finishedState);

            //_stateMachine.ChangeState(ExperimentStateType.Setup);

            _signalBus.Subscribe<ChangeStateSignal<ExperimentStateType>>(OnChangeState);
        }

        private void OnChangeState(ChangeStateSignal<ExperimentStateType> signal)
        {
            _logger.LogWarning($"Changing state to {signal.NextState}");
            _stateMachine.ChangeState(signal.NextState);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ChangeStateSignal<ExperimentStateType>>(OnChangeState);
        }
    }
}
