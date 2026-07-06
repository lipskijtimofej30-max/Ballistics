using System;
using System.Linq;
using DefaultNamespace;
using Game.Scripts.Core;
using Game.Scripts.Infrastructure.Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Scripts.View
{
    public class ShapeDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;
        
        private ProjectileSettings _projectileSettings;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(ProjectileSettings projectileSettings, SignalBus signalBus)
        {
            _projectileSettings = projectileSettings;
            _signalBus = signalBus;
        }

        private void Start()
        {
            _dropdown.ClearOptions();
            
            string[] options = Enum.GetNames(typeof(ShapeType));
            _dropdown.AddOptions(options.ToList());
            _dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            _dropdown.SetValueWithoutNotify(0);
            OnDropdownValueChanged(0);
        }
        
        private void OnDropdownValueChanged(int value)
        {
            _projectileSettings.ShapeType = (ShapeType)value;
            _signalBus.Fire(new ShapeDropdownChangedSignal(_projectileSettings.ShapeType));
            _signalBus.Fire<ProjectileSettingsChangedSignal>();
        }

        private void OnDestroy()
        {
            _dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        }
    }
}