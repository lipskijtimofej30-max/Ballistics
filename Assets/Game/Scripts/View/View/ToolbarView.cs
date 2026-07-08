using Assets.Game.Scripts.Infrastructure.GameStateMachine;
using Assets.Game.Scripts.Infrastructure.Signals;
using DefaultNamespace;
using Game.Scripts.Core;
using Game.Scripts.Infrastructure.GameStateMachine;
using Game.Scripts.Infrastructure.Signals;
using Game.Scripts.UX;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.View.View
{
    public class ToolbarView : MonoBehaviour
    {
        [Header("Buttons")] [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _stopButton;
        [SerializeField] private Button _laboratoryButton;
        [SerializeField] private Button _experimentButton;

        [Header("Label Buttons")] [SerializeField]
        private TMP_Text _createButtonLabel;

        private SignalBus _signalBus;
        private Simulator _simulator;
        private ModeController _modeController;

        public Button PauseButton => _pauseButton;
        public Button CreateButton => _createButton;
        public Button StartButton => _startButton;
        public Button StopButton => _stopButton;
        public Button LaboratoryButton => _laboratoryButton;
        public Button ExperimentButton => _experimentButton;

        public TMP_Text CreateButtonLabel => _createButtonLabel;

        [Inject]
        private void Construct(SignalBus signalBus, Simulator simulator, ModeController modeController)
        {
            _signalBus = signalBus;
            _simulator = simulator;
            _modeController = modeController;
        }

        private void Start()
        {
            _signalBus.Subscribe<SetupDirtyStatusChangedSignal>(OnDirtyStatusChanged);

            _createButton.onClick.AddListener(() =>
            {
                if (_modeController.CurrentMode == AppMode.Laboratory)
                {
                    _signalBus.Fire(new ChangeStateSignal<SimulationStateType>(SimulationStateType.SetupSimulation));
                    _simulator.Spawn();
                    _signalBus.Fire(new CleanSetupRequestedSignal());
                }
            });

            _startButton.onClick.AddListener(() =>
            {
                if (_modeController.CurrentMode == AppMode.Laboratory)
                    _signalBus.Fire(new ChangeStateSignal<SimulationStateType>(SimulationStateType.Simulation));
                else
                    _signalBus.Fire(new ChangeStateSignal<ExperimentStateType>(ExperimentStateType.Running));
            });

            _pauseButton.onClick.AddListener(() =>
            {
                if (_modeController.CurrentMode == AppMode.Laboratory)
                    _signalBus.Fire(new ChangeStateSignal<SimulationStateType>(SimulationStateType.PausedSimulation));
                else
                    _signalBus.Fire(new ChangeStateSignal<ExperimentStateType>(ExperimentStateType.Pause));
            });

            _stopButton.onClick.AddListener(() =>
            {
                if (_modeController.CurrentMode == AppMode.Laboratory)
                    _signalBus.Fire(new ChangeStateSignal<SimulationStateType>(SimulationStateType.FinishedSimulation));
                else
                    _signalBus.Fire(new ChangeStateSignal<ExperimentStateType>(ExperimentStateType.Finished));
            });

            _laboratoryButton.onClick.AddListener(SwitchToLaboratory);
            _experimentButton.onClick.AddListener(SwitchToExperiment);
        }

        private void SwitchToLaboratory()
        {
            _modeController.SwitchTo(AppMode.Laboratory);
            _signalBus.Fire(new ChangeStateSignal<SimulationStateType>(SimulationStateType.SetupSimulation));
        }

        private void SwitchToExperiment()
        {
            _modeController.SwitchTo(AppMode.Experiment);
            _signalBus.Fire(new ChangeStateSignal<ExperimentStateType>(ExperimentStateType.Setup));
        }

        private void OnDirtyStatusChanged(SetupDirtyStatusChangedSignal signal)
        {
            if (signal.IsDirty)
            {
                _createButtonLabel.text = "Обновить";
                _startButton.interactable = false;
            }
            else
            {
                _createButtonLabel.text = "+ Создать";
                _startButton.interactable = true;
            }
        }

        private void OnDestroy()
        {
            _signalBus?.TryUnsubscribe<SetupDirtyStatusChangedSignal>(OnDirtyStatusChanged);

            _pauseButton.onClick?.RemoveAllListeners();
            _createButton.onClick?.RemoveAllListeners();
            _startButton.onClick?.RemoveAllListeners();
            _stopButton.onClick?.RemoveAllListeners();
            _laboratoryButton.onClick?.RemoveAllListeners();
            _experimentButton.onClick?.RemoveAllListeners();
        }
    }
}