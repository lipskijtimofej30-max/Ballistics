using System;
using DefaultNamespace;
using Game.Scripts.Core;
using UnityEngine;
using Zenject;

namespace Game.Scripts.View.UseCase
{
    public class BaseProjectileUseCase : IDisposable
    {
        private BaseProjectileView _baseProjectileView;
        private SignalBus _signalBus;
        private ProjectileSettings _projectileSettings;
        private MassCalculator _massCalculator;

        [Inject]
        private void Construct(BaseProjectileView baseProjectileView, ProjectileSettings projectileSettings, MassCalculator massCalculator, SignalBus signalBus)
        {
            _baseProjectileView = baseProjectileView;
            _projectileSettings = projectileSettings;
            _massCalculator = massCalculator;
            _signalBus = signalBus;
            
            _baseProjectileView.SizeSlider.onValueChanged.AddListener(ChangeSizeValue);
            _baseProjectileView.DensitySlider.onValueChanged.AddListener(ChangeDensityValue);
            
            ChangeSizeValue(_baseProjectileView.SizeSlider.value);
            ChangeDensityValue(_baseProjectileView.DensitySlider.value);
            
            _signalBus.Subscribe<SimulationSettingsChangedSignal>(Refresh);
            Refresh();
        }

        private void ChangeDensityValue(float value)
        {
            _projectileSettings.Density = (int)value;
            _signalBus.Fire<SimulationSettingsChangedSignal>();
        }

        private void ChangeSizeValue(float value)
        {
            _projectileSettings.Size = value;
            _signalBus.Fire<SimulationSettingsChangedSignal>();
        }
        private void Refresh()
        {
            _baseProjectileView.SetSizeText(_projectileSettings.Size.ToString("F2"));
            _baseProjectileView.SetDensityText(_projectileSettings.Density.ToString());

            float mass = _massCalculator.GetMass(_projectileSettings.ShapeType);

            _baseProjectileView.SetMassText(mass.ToString("F2"));
        }
        public void Dispose()
        {
            _baseProjectileView.SizeSlider.onValueChanged.RemoveListener(ChangeSizeValue);
            _baseProjectileView.DensitySlider.onValueChanged.RemoveListener(ChangeDensityValue);
            
            _signalBus.Unsubscribe<SimulationSettingsChangedSignal>(Refresh);
        }
    }
}