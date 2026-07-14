using System;
using Assets.Game.Scripts.Infrastructure.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.UX
{
    public class CancelButtonPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _button;
        [SerializeField] private bool _initialState = false;
        
        private SignalBus  _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void Start()
        {
            _panel.SetActive(_initialState);
            _button.onClick.AddListener(() => _signalBus.Fire(new OverflowLineSignal(false)));
        }
    }
}