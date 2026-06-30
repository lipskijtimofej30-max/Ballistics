using System;
using Game.Scripts.Core;
using UnityEngine;
using Zenject;

namespace Game.Scripts.View.UseCase
{
    public class BaseProjectileUseCase : IDisposable
    {
        private BaseProjectileView _baseProjectileView;
        private ProjectileSettings _projectileSettings;
        private MassCalculator _massCalculator;

        [Inject]
        private void Construct(BaseProjectileView baseProjectileView, ProjectileSettings projectileSettings, MassCalculator massCalculator)
        {
            _baseProjectileView = baseProjectileView;
            _projectileSettings = projectileSettings;
            _massCalculator = massCalculator;
            
            _baseProjectileView.SizeSlider.onValueChanged.AddListener(ChangeSizeValue);
            _baseProjectileView.DensitySlider.onValueChanged.AddListener(ChangeDensityValue);
            
            _baseProjectileView.SizeSlider.onValueChanged.AddListener(ChangeMassText);
            _baseProjectileView.DensitySlider.onValueChanged.AddListener(ChangeMassText);
            
            ChangeSizeValue(_baseProjectileView.SizeSlider.value);
            ChangeDensityValue(_baseProjectileView.DensitySlider.value);
            ChangeMassText(_massCalculator.GetMass(_projectileSettings.ShapeType));
        }

        private void ChangeDensityValue(float value)
        {
            _baseProjectileView.SetDensityText(value.ToString("F2"));
            _projectileSettings.Density = (int)value;
            Debug.Log($"Density change to: {_projectileSettings.Density}");
        }

        private void ChangeSizeValue(float value)
        {
            _baseProjectileView.SetSizeText(value.ToString("F2"));
            _projectileSettings.Size = value;
            Debug.Log($"Size change to: {_projectileSettings.Size}");
        }

        private void ChangeMassText(float value)
        {
            _baseProjectileView.SetMassText(_massCalculator.GetMass(_projectileSettings.ShapeType).ToString("F2"));
        }
        public void Dispose()
        {
            _baseProjectileView.SizeSlider.onValueChanged.RemoveListener(ChangeSizeValue);
            _baseProjectileView.DensitySlider.onValueChanged.RemoveListener(ChangeDensityValue);
        }
    }
}