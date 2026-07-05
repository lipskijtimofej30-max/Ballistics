using System;
using Game.Scripts.Core;
using Game.Scripts.Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace Game.Scripts.View.UseCase
{
    public class EnvironmentUseCase : IDisposable
    {
        private readonly FloatParameterBinder _gravityBinder;
        private readonly FloatParameterBinder _airSpeedBinder;
        private readonly FloatParameterBinder _airDensityBinder;
        
        private readonly EnvironmentView _environmentView;
        private readonly EnvironmentSettings _environmentSettings;
        private readonly SignalBus _signalBus;

        public EnvironmentUseCase(EnvironmentView environmentView, EnvironmentSettings environmentSettings, SignalBus signalBus)
        {
            _environmentView = environmentView;
            _environmentSettings = environmentSettings;
            _signalBus = signalBus;
            
            _gravityBinder = new FloatParameterBinder(
                _environmentView.GravityParameter,
                4f,
                20f,
                "F2",
                () => _environmentSettings.Gravity.y,
                x => _environmentSettings.Gravity = new Vector3(0f, -x, 0f),
                () => signalBus.Fire<EnvironmentSettingsChangedSignal>());
            
            _airSpeedBinder = new FloatParameterBinder(
                _environmentView.AirSpeedParameter,
                -20f,
                20f,
                "F2",
                () => _environmentSettings.WindVelocity.x,
                x => _environmentSettings.WindVelocity = new Vector3(x, 0f, 0f),
                () => signalBus.Fire<EnvironmentSettingsChangedSignal>());
            
            _airDensityBinder = new FloatParameterBinder(
                _environmentView.AirDensityParameter,
                1.1f,
                1.25f,
                "F2",
                () => _environmentSettings.AirDensity,
                x => _environmentSettings.AirDensity = x,
                () => signalBus.Fire<EnvironmentSettingsChangedSignal>());
            
            _environmentView.AirResistanceToggle.onValueChanged.AddListener(ToggleContainer);
        }

        private void ToggleContainer(bool enabled)
        {
            _environmentSettings.AirResistanceEnabled = enabled;
            if (enabled) _environmentView.ShowContainer();
            else _environmentView.HideContainer();
            _signalBus.Fire(new EnvironmentSettingsChangedSignal());                                      
        }

        public void Dispose()
        {
            _gravityBinder?.Dispose();
            _airSpeedBinder?.Dispose();
            _airDensityBinder?.Dispose();
        }
    }
}