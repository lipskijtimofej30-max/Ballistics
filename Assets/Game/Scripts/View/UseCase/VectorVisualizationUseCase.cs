using System;
using Game.Scripts.Core;
using Game.Scripts.Settings;
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.View.UseCase
{
    public class VectorVisualizationUseCase : IDisposable
    {
        private readonly VectorVisualizationSettings _settings;
        private readonly VectorVisualizationView _view;
        
        private FloatParameterBinder _scaleLengthBinder;

        [Inject]
        public VectorVisualizationUseCase(VectorVisualizationSettings settings, VectorVisualizationView view)
        {
            _settings = settings;
            _view = view;

            _scaleLengthBinder = new FloatParameterBinder(
                _view.ScaleParameter, 0.1f, 2f, "F2",
                () => _settings.ScaleLength, x => _settings.ScaleLength = x, () => { });
            
            _view.VelocityToggle.onValueChanged.AddListener(x => _settings.IsActiveVelocity = x);
            _view.AccelerationToggle.onValueChanged.AddListener(x => _settings.IsActiveAcceleration = x);
            _view.ForceToggle.onValueChanged.AddListener(x => _settings.IsActiveTotalForce = x);
            
            _view.VelocityToggle.SetIsOnWithoutNotify(_settings.IsActiveVelocity);
            _view.AccelerationToggle.SetIsOnWithoutNotify(_settings.IsActiveAcceleration);
            _view.ForceToggle.SetIsOnWithoutNotify(_settings.IsActiveTotalForce);
        }

        public void Dispose()
        {
            _scaleLengthBinder?.Dispose();
            _view.VelocityToggle.onValueChanged.RemoveAllListeners();
            _view.AccelerationToggle.onValueChanged.RemoveAllListeners();
            _view.ForceToggle.onValueChanged.RemoveAllListeners();
        }
    }
}