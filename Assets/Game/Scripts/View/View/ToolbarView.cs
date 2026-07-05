using DefaultNamespace;
using Game.Scripts.Core;
using Game.Scripts.Infrastructure.GameStateMachine;
using Game.Scripts.UX;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.View.View
{
    public class ToolbarView : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _stopButton;
        [Header("Label Buttons")]
        [SerializeField] private TMP_Text _createButtonLabel;
        
        private SignalBus _signalBus;
        private Simulator _simulator;
        private SetupDirtyTracker _dirtyTracker;
        
        public Button PauseButton => _pauseButton;
        public Button CreateButton => _createButton;
        public Button StartButton => _startButton;
        public Button StopButton => _stopButton;
        
        public TMP_Text CreateButtonLabel => _createButtonLabel;

        [Inject]
        private void Construct(SignalBus signalBus, Simulator simulator, SetupDirtyTracker dirtyTracker)
        {
            _signalBus = signalBus;
            _simulator = simulator;
            _dirtyTracker = dirtyTracker;
        }

        private void Start()
        {
            _createButton.onClick.AddListener( 
                () =>
                {
                    _signalBus.Fire(new ChangeStateSignal(GameStateType.SetupSimulation));
                    _simulator.Spawn();
                    _dirtyTracker.MarkClean();
                });
            
            _startButton.onClick.AddListener( 
                () => _signalBus.Fire(new ChangeStateSignal(GameStateType.Simulation)));
            
            _pauseButton.onClick.AddListener(
                () => _signalBus.Fire(new ChangeStateSignal(GameStateType.PausedSimulation)));
            
            _stopButton.onClick.AddListener(
                () => _signalBus.Fire(new ChangeStateSignal(GameStateType.FinishedSimulation)));
        }
        
        private void OnDestroy()
        {
            _pauseButton.onClick.RemoveAllListeners();
            _createButton.onClick.RemoveAllListeners();
            _startButton.onClick.RemoveAllListeners();
            _stopButton.onClick.RemoveAllListeners();
        }
    }
}