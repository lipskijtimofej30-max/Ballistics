using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Core;
using Game.Scripts.Infrastructure.Signals;
using Game.Scripts.Settings;
using Game.Scripts.View.View;
using UnityEngine;
using Zenject;

namespace Game.Scripts.View.UseCase
{
    public class VisualizationUseCase : IDisposable
    {
        private readonly List<ColorOption> _colors = new List<ColorOption>
        {
            new ColorOption { Name = "Красный", Color = Color.red },
            new ColorOption { Name = "Розовый", Color = Color.magenta },
            new ColorOption { Name = "Серый", Color = Color.gray },
            new ColorOption { Name = "Жёлтый", Color = Color.yellow },
            new ColorOption { Name = "Белый", Color = Color.white },
        };
        private readonly VisualizationView _view;
        private readonly VisualizationSettings _settings;
        private readonly SignalBus _signalBus;
        private readonly FloatParameterBinder _widthBinder;
        
        private bool _previewAllowedByState = true;

        private readonly TrajectoryRenderer _liveRenderer;
        private readonly TrajectoryRenderer _previewRenderer;

        [Inject]
        public VisualizationUseCase(VisualizationView view,
            VisualizationSettings settings,
            SignalBus signalBus,
            [Inject(Id = "Live")] TrajectoryRenderer liveRenderer,
            [Inject(Id = "Preview")] TrajectoryRenderer previewRenderer)
        {
            _view = view;
            _settings = settings;
            _signalBus = signalBus;
            _liveRenderer = liveRenderer;
            _previewRenderer = previewRenderer;

            _widthBinder = new FloatParameterBinder(
                view.WidthParameter,
                0.1f,
                1f,
                "F2",
                () => _settings.Width,
                x => _settings.Width = x,
                () => signalBus.Fire<VisualizationSettingsChangedSignal>()
            );
            
            InitializeDropdown();
            
            _view.PreviewToggle.SetIsOnWithoutNotify(_settings.VisiblePreview); // синхронизация начального состояния
            _view.PreviewToggle.onValueChanged.AddListener(OnPreviewVisible);
            _view.ColorDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
             
            signalBus.Subscribe<VisualizationSettingsChangedSignal>(SetTrajectorySettings);
            SetTrajectorySettings();
        }

        private void InitializeDropdown()
        {
            _view.ColorDropdown.ClearOptions();
            List<string> options = _colors.Select(c => c.Name).ToList();
            _view.ColorDropdown.AddOptions(options);
            string currentColorName = _colors.Find(c => c.Color == _settings.Color).Name;
            int index = _colors.FindIndex(c => c.Name == currentColorName);
            _view.ColorDropdown.value = index >= 0 ? index : 0;
        }

        private void OnDropdownValueChanged(int value)
        {
            if (value < 0 || value >= _colors.Count) return;

            Color selectedColor = _colors[value].Color;
            _settings.Color = selectedColor;
            _signalBus.Fire<VisualizationSettingsChangedSignal>();
        }

        public void SetPreviewAllowed(bool allowed)
        {
            _previewAllowedByState = allowed;
            UpdatePreviewVisibility();
        }

        private void OnPreviewVisible(bool visible)
        {
            _settings.VisiblePreview = visible;
            UpdatePreviewVisibility();
        }

        private void UpdatePreviewVisibility()
        {
            _previewRenderer.SetVisible(_previewAllowedByState && _settings.VisiblePreview);
        }

        private void SetTrajectorySettings()
        {
            _liveRenderer.SetSettings(_settings.Width, _settings.Color);
            _previewRenderer.SetSettings(_settings.Width, _settings.Color);
        }
        
        public void Dispose()
        {
            _widthBinder.Dispose();
            _view.PreviewToggle.onValueChanged.RemoveListener(OnPreviewVisible);
            _view.ColorDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
            _signalBus.TryUnsubscribe<VisualizationSettingsChangedSignal>(SetTrajectorySettings);
        }
    }
    
    [Serializable]
    public struct ColorOption
    {
        public string Name;
        public Color Color;
    }
}