using System;
using DefaultNamespace;
using Game.Scripts.Core;
using Game.Scripts.Infrastructure.GameStateMachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.View.View
{
    public class ToolbarView : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _stopButton;
        
        private SignalBus _signalBus;
        private Simulator _simulator;

        [Inject]
        private void Construct(SignalBus signalBus, Simulator simulator)
        {
            _signalBus = signalBus;
            _simulator = simulator;
        }

        private void Start()
        {
            _createButton.onClick.AddListener( 
                () =>
                {
                    _signalBus.Fire(new ChangeStateSignal(GameStateType.SetupSimulation));
                    _simulator.Spawn();
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