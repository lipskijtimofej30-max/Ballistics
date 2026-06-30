using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.View
{
    public class ConfirmButtonHandler : MonoBehaviour
    {
        private Button _button;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _signalBus.Fire(new ConfirmButtonClickSignal());
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}