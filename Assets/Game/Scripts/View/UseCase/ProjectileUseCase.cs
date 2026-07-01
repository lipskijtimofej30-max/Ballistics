using System;
using DefaultNamespace;
using Game.Scripts.Core;
using UnityEngine;
using Zenject;

namespace Game.Scripts.View.UseCase
{
    public class ProjectileUseCase : IDisposable
    {
        private ProjectileView _projectileView;
        private SignalBus _signalBus;
        private ProjectileSettings _projectileSettings;
        private MassCalculator _massCalculator;

        [Inject]
        private void Construct(ProjectileView projectileView, ProjectileSettings projectileSettings, MassCalculator massCalculator, SignalBus signalBus)
        {
            _projectileView = projectileView;
            _projectileSettings = projectileSettings;
            _massCalculator = massCalculator;
            _signalBus = signalBus;
            
            _projectileView.SizeSlider.onValueChanged.AddListener(ChangeSizeValue);
            _projectileView.DensitySlider.onValueChanged.AddListener(ChangeDensityValue);
            
            ChangeSizeValue(_projectileView.SizeSlider.value);
            ChangeDensityValue(_projectileView.DensitySlider.value);
            
            _signalBus.Subscribe<ProjectileSettingsChangedSignal>(Refresh);
            Refresh();
        }

        private void ChangeDensityValue(float value)
        {
            _projectileSettings.Density = (int)value;
            _signalBus.Fire<ProjectileSettingsChangedSignal>();
        }

        private void ChangeSizeValue(float value)
        {
            _projectileSettings.Size = value;
            _signalBus.Fire<ProjectileSettingsChangedSignal>();
        }
        private void Refresh()
        {
            _projectileView.SetSizeText(_projectileSettings.Size.ToString("F2"));
            _projectileView.SetDensityText(_projectileSettings.Density.ToString());

            float mass = _massCalculator.GetMass(_projectileSettings.ShapeType);

            _projectileView.SetMassText(mass.ToString("F2"));
        }
        public void Dispose()
        {
            _projectileView.SizeSlider.onValueChanged.RemoveListener(ChangeSizeValue);
            _projectileView.DensitySlider.onValueChanged.RemoveListener(ChangeDensityValue);
            
            _signalBus.Unsubscribe<ProjectileSettingsChangedSignal>(Refresh);
        }
    }
}