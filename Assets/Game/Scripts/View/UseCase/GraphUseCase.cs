using Assets.Game.Scripts.Core.Graphics;
using Assets.Game.Scripts.Infrastructure.Signals;
using Assets.Game.Scripts.Settings;
using Game.Scripts.Core;
using Game.Scripts.View.View;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Zenject;

namespace Assets.Game.Scripts.View.UseCase
{
    public class GraphUseCase : IDisposable
    {
        private readonly GraphView _view;
        private readonly GraphSettings _settings;
        private readonly SignalBus _signalBus;

        private FloatParameterBinder _countLabelXBinder;
        private FloatParameterBinder _countLabelYBinder;

        public event Action<GraphType> OnGraphTypeSelected;

        public GraphUseCase(GraphView view, GraphSettings graphSettings, SignalBus signalBus)
        {
            _view = view;
            _settings = graphSettings;
            _signalBus = signalBus;

            _view.Dropdown.ClearOptions();
            var options = new List<string>();
            foreach (GraphType type in Enum.GetValues(typeof(GraphType)))
                options.Add(type.GetDisplayName());
            _view.Dropdown.AddOptions(options);

            _countLabelXBinder = new FloatParameterBinder(
                _view.CountLabelX,3f, 9f,"F2",
                () => _settings.CountLabelX,
                x => _settings.CountLabelX = (int)x,
                () => _signalBus.Fire(new GraphSettingsChangedSignal()));

            _countLabelYBinder = new FloatParameterBinder(
                _view.CountLabelY, 2f, 5f, "F2",
                () => _settings.CountLabelY,
                x => _settings.CountLabelY = (int)x,
                () => _signalBus.Fire(new GraphSettingsChangedSignal()));

            _view.Dropdown.onValueChanged.AddListener(index =>
            {
                OnGraphTypeSelected?.Invoke((GraphType)index);
            });
        }

        public void Dispose()
        {
            _countLabelXBinder?.Dispose();
            _countLabelYBinder?.Dispose();
        }
    }
}
