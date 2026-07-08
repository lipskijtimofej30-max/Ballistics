using Game.Scripts.Infrastructure.GameStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Scripts.Infrastructure.Logger;
using Game.Scripts.Infrastructure.Signals;
using UnityEngine.UIElements;
using Zenject;

namespace Assets.Game.Scripts.Infrastructure.GameStateMachine
{
    public class ModeController : ITickable
    {
        private readonly GameStateMachine<SimulationStateType> _laboratoryMachine;
        private readonly GameStateMachine<ExperimentStateType> _experimentMachine;
        private readonly ILogger _logger;
        private readonly SignalBus _signalBus;

        public AppMode CurrentMode { get; private set; } = AppMode.Laboratory;

        [Inject]
        public ModeController(GameStateMachine<SimulationStateType> laboratoryMachine,
            GameStateMachine<ExperimentStateType> experimentMachine, ILogger logger, SignalBus signalBus)
        {
            _laboratoryMachine = laboratoryMachine;
            _experimentMachine = experimentMachine;
            _logger = logger;
            _signalBus = signalBus;
        }

        public void SwitchTo(AppMode mode)
        {
            if (mode == CurrentMode) return;

            if (CurrentMode == AppMode.Laboratory)
                _laboratoryMachine.ChangeState(SimulationStateType.SetupSimulation);
            else
                _experimentMachine.ChangeState(ExperimentStateType.Setup);
            
            _logger.LogWarning($"Mode {CurrentMode} changed to {mode}");
            _signalBus.Fire(new ChangeAppModeSignal(mode));
            CurrentMode = mode;
        }

        public void Tick()
        {
            if(CurrentMode == AppMode.Laboratory)
                _laboratoryMachine.Tick();
            else
                _experimentMachine.Tick();
        }
    }
}
