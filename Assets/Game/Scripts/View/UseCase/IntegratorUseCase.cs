using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Core;
using Game.Scripts.Infrastructure.Logger;
using Game.Scripts.Infrastructure.Signals;
using Game.Scripts.Settings;
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.View.UseCase
{
    public class IntegratorUseCase : IDisposable
    {
        private readonly IntegratorView _integratorView;
        private readonly IntegratorSettings _integratorSettings;
        private readonly SignalBus _signalBus;
        private readonly ILogger _logger;
        
        private readonly FloatParameterBinder _dtBinder;
        private readonly FloatParameterBinder _timeScaleBinder;

        [Inject]
        public IntegratorUseCase(IntegratorView integratorView, IntegratorSettings integratorSettings, SignalBus signalBus, ILogger logger)
        {
            _integratorView = integratorView;
            _integratorSettings = integratorSettings;
            _signalBus = signalBus;
            _logger = logger;

            _dtBinder = new FloatParameterBinder(
                _integratorView.DtParameter,
                0.001f,
                0.03f,
                "F3",
                () => integratorSettings.IntegrationStep,
                x => integratorSettings.IntegrationStep = x,
                () => signalBus.Fire(new IntegratorSettingsChangedSignal()));

            _timeScaleBinder = new FloatParameterBinder(
                _integratorView.TimeScaleParameter,
                0.5f,
                2f,
                "F2",
                () => integratorSettings.TimeScale,
                x => integratorSettings.TimeScale = x,
                () => signalBus.Fire(new IntegratorSettingsChangedSignal()));
            
            InitializeDropdown();
            
            _integratorView.IntegratorMethodDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        private void InitializeDropdown()
        {
            _integratorView.IntegratorMethodDropdown.ClearOptions();
            List<string> options = Enum.GetNames(typeof(IntegratorMethod)).ToList();
            _integratorView.IntegratorMethodDropdown.AddOptions(options);
            _integratorView.IntegratorMethodDropdown.value = 1;
        }

        private void OnDropdownValueChanged(int value)
        {
            if(value < 0 ) return;
            _integratorSettings.IntegratorMethod = (IntegratorMethod)value;
            _logger.Log($"Integrator method {_integratorSettings.IntegratorMethod} has been changed");
            _signalBus.Fire(new IntegratorSettingsChangedSignal());
        }

        public void Dispose()
        {
            _dtBinder?.Dispose();
            _integratorView.IntegratorMethodDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        }
    }
}