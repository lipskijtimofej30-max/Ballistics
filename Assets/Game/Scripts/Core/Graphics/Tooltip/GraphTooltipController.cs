using System;
using System.Collections.Generic;
using Assets.Game.Scripts.Core.Experiment;
using Assets.Game.Scripts.Infrastructure.GameStateMachine;
using Game.Scripts.View.View;
using UnityEngine;
using Zenject;
using ILogger = Game.Scripts.Infrastructure.Logger.ILogger;

namespace Assets.Game.Scripts.Core.Graphics
{
    public class GraphTooltipController : IDisposable
    {
        private readonly GraphTooltipView _view;
        private readonly IGraphInfoProvider _infoProvider;
        private readonly ModeController _modeController;
        private readonly ExperimentSession _session;
        private readonly ExperimentGraphFilterController _filterController;
        private readonly ILogger _logger;

        private readonly Dictionary<AppMode, ITooltipStrategy> _strategies;

        public bool Enabled { get; set; } = true;

        [Inject]
        public GraphTooltipController(
            GraphTooltipView view,
            IGraphInfoProvider infoProvider,
            ModeController modeController,
            ILogger logger,
            ExperimentSession session,
            ExperimentGraphFilterController filterController)
        {
            _view = view;
            _infoProvider = infoProvider;
            _modeController = modeController;
            _logger = logger;
            _session = session;
            _filterController = filterController;

            _strategies = new Dictionary<AppMode, ITooltipStrategy>
            {
                { AppMode.Experiment, new ExperimentTooltipStrategy(_session,  _filterController) },
                { AppMode.Laboratory, new LaboratoryTooltipStrategy(hoverThreshold: 0.05f) }
            };

            _view.OnPointerMoved += HandlePointerMoved;
            _view.OnPointerExited += HandlePointerExited;
        }

        private void HandlePointerMoved(Vector2 normalizedPos)
        {
            if (!Enabled) return;

            var sources = _infoProvider.DataSources;
            if (sources == null || sources.Count == 0)
            {
                _view.Hide();
                return;
            }

            AppMode currentMode = _modeController.CurrentMode;

            if (!_strategies.TryGetValue(currentMode, out var activeStrategy))
            {
                _logger.LogWarning($"Нет зарегистрированной стратегии тултипа для режима: {currentMode}");
                _view.Hide();
                return;
            }

            try
            {
                Vector2 min = _infoProvider.CurrentMin;
                Vector2 max = _infoProvider.CurrentMax;

                if (activeStrategy.TryGetTooltipData(normalizedPos, sources, min, max, out string tooltipText,
                        out Vector2 dotPos))
                {
                    _view.Show(tooltipText);
                    _view.UpdateDotPosition(dotPos);
                }
                else
                {
                    _view.Hide();
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[TooltipController] Ошибка расчетов: {e.Message}");
            }
        }

        private void HandlePointerExited() => _view.Hide();

        public void Dispose()
        {
            _view.OnPointerMoved -= HandlePointerMoved;
            _view.OnPointerExited -= HandlePointerExited;
        }
    }
}