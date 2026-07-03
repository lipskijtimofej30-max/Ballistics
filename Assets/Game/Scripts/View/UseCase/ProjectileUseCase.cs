using System;
using DefaultNamespace;
using Game.Scripts.Core;
using Zenject;

namespace Game.Scripts.View.UseCase
{
    public class ProjectileUseCase : IDisposable
    {
        private readonly FloatParameterBinder _sizeBinder;
        private readonly FloatParameterBinder _densityBinder;

        private readonly SignalBus _signalBus;
        private readonly ProjectileSettings _settings;
        private readonly ProjectileView _view;
        private readonly MassCalculator _calculator;

        [Inject]
        public ProjectileUseCase(
            ProjectileView view,
            ProjectileSettings settings,
            MassCalculator calculator,
            SignalBus signalBus)
        {
            _view = view;
            _settings = settings;
            _calculator = calculator;
            _signalBus = signalBus;

            _sizeBinder = new FloatParameterBinder(
                _view.SizeParameter,
                0.05f,
                1f,
                "F2",
                () => _settings.Size,
                x => _settings.Size = x,
                () => _signalBus.Fire<ProjectileSettingsChangedSignal>());

            _densityBinder = new FloatParameterBinder(
                _view.DensityParameter,
                100,
                22650,
                "F2",
                () => _settings.Density,
                x => _settings.Density = x,
                () => _signalBus.Fire<ProjectileSettingsChangedSignal>());

            Refresh();
            _signalBus.Subscribe<ProjectileSettingsChangedSignal>(Refresh);
        }

        private void Refresh()
        {
            float mass = _calculator.GetMass(_settings.ShapeType, _settings.Density, _settings.Size);

            _view.SetMassText(mass.ToString("F2"));
        }

        public void Dispose()
        {
            _sizeBinder.Dispose();
            _densityBinder.Dispose();
            _signalBus.Unsubscribe<ProjectileSettingsChangedSignal>(Refresh);
        }
    }
}