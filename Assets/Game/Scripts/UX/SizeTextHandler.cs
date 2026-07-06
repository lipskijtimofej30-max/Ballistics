using System;
using Game.Scripts.Core;
using Game.Scripts.Infrastructure.Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Scripts.UX
{
    public class SizeTextHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text _sizeText;
        
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus, ProjectileSettings projectileSettings)
        {
            _signalBus = signalBus;
            
            OnChangedValue(new ShapeDropdownChangedSignal(projectileSettings.ShapeType));
            _signalBus.Subscribe<ShapeDropdownChangedSignal>(OnChangedValue);
        }
        
        private void OnChangedValue(ShapeDropdownChangedSignal signal)
        {
            string text = signal.Value switch
            {
                ShapeType.Cube => "Грань",
                ShapeType.Sphere => "Радиус",
                _ => "Размер"
            };
            _sizeText.text = text;
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<ShapeDropdownChangedSignal>(OnChangedValue);
        }
    }
}