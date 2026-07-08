using Game.Scripts.Infrastructure.GameStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using Zenject;

namespace Assets.Game.Scripts.Infrastructure.GameStateMachine
{
    public class ModeController : ITickable
    {
        private readonly GameStateMachine<SimulationStateType> _laboratoryMachine;
        private readonly GameStateMachine<ExperimentStateType> _experimentMachine;

        public AppMode CurrentMode { get; private set; } = AppMode.Laboratory;

        [Inject]
        public ModeController(GameStateMachine<SimulationStateType> laboratoryMachine,
            GameStateMachine<ExperimentStateType> experimentMachine)
        {
            _laboratoryMachine = laboratoryMachine;
            _experimentMachine = experimentMachine;
        }

        public void SwitchTo(AppMode mode)
        {
            if (mode == CurrentMode) return;

            if (CurrentMode == AppMode.Laboratory)
                _laboratoryMachine.ChangeState(SimulationStateType.SetupSimulation);
            else
                _experimentMachine.ChangeState(ExperimentStateType.Setup);

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
