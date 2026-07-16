using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Game.Scripts.Core.Experiment.Parameter;
using Assets.Game.Scripts.Settings;
using Game.Scripts.Core;
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.View.UseCase
{
    public class ExperimentSettingsUseCase : IDisposable
    {
        private readonly ExperimentParameterDataBase _parameters;
        private readonly ExperimentSettings  _settings;
        private readonly ExperimentSettingsView _view;
          
        private readonly FloatParameterBinder _minBinder;
        private readonly FloatParameterBinder _maxBinder;
        private readonly FloatParameterBinder _stepBinder;
        private readonly FloatParameterBinder _pauseBinder;

        [Inject]
        public ExperimentSettingsUseCase(ExperimentParameterDataBase parameters, ExperimentSettings settings, ExperimentSettingsView view)
        {
            _parameters = parameters;
            _settings = settings;
            _view = view;
            
            _view.DropdownParameter.ClearOptions();
            _view.DropdownParameter.AddOptions(_parameters.Parameters.Select(p => p.DisplayName).ToList());
            _view.DropdownParameter.SetValueWithoutNotify(_settings.SelectedParameterIndex);
            _view.DropdownParameter.onValueChanged.AddListener(OnParameterChanged);
            
            var firstParameter = _parameters.GetCurrentParameter();
            
            _minBinder = new FloatParameterBinder(_view.MinParameter, /* границы */ firstParameter.MinRangeValue, firstParameter.MaxRangeValue, "F2",
                () => _settings.MinValue, x => _settings.MinValue = x, () => { });
            _maxBinder = new FloatParameterBinder(_view.MaxParameter, firstParameter.MinRangeValue, firstParameter.MaxRangeValue, "F2",
                () => _settings.MaxValue, x => _settings.MaxValue = x, () => { });
            _stepBinder = new FloatParameterBinder(_view.StepParameter, 1f, 20f, "F2",
                () => _settings.Step, x => _settings.Step = x, () => { });
            _pauseBinder = new FloatParameterBinder(_view.PauseParameter, 0.5f, 5f, "F2",
                () => _settings.PauseBetweenRuns, x => _settings.PauseBetweenRuns = x, () => { });
        }

        private void OnParameterChanged(int index)
        {
            _settings.SelectedParameterIndex = index;
            var selected = _parameters.Parameters[index];

            _minBinder.UpdateBounds(
                selected.MinRangeValue, 
                selected.MaxRangeValue, 
                selected.Unit, 
                selected.MinRangeValue);
            
            _maxBinder.UpdateBounds(
                selected.MinRangeValue, 
                selected.MaxRangeValue, 
                selected.Unit, 
                selected.MaxRangeValue);
        }
        
        public void Dispose()
        {
            _minBinder?.Dispose();
            _maxBinder?.Dispose();
            _stepBinder?.Dispose();
            _pauseBinder?.Dispose();
        }
    }
}