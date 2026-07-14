using System;
using System.Collections.Generic;
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
        private readonly ILogger _logger;

        private const float HoverThreshold = 0.05f;

        [Inject]
        public GraphTooltipController(GraphTooltipView view, IGraphInfoProvider infoProvider,
            ModeController modeController, ILogger logger)
        {
            _view = view;
            _infoProvider = infoProvider;
            _modeController = modeController;
            _logger = logger;

            _view.OnPointerMoved += HandlePointerMoved;
            _view.OnPointerExited += HandlePointerExited;
        }

        private void HandlePointerMoved(Vector2 normalizedPos)
        {
            var sources = _infoProvider.DataSources;
            if (sources == null || sources.Count == 0)
            {
                _view.Hide();
                return;
            }

            if (_modeController.CurrentMode == AppMode.Experiment)
                ShowExperimentMode(normalizedPos, sources);
            else
                ShowLaboratoryMode(normalizedPos, sources);
        }

        private void ShowExperimentMode(Vector2 normalizedPos, IReadOnlyList<IGraphDataSource> sources)
        {
            try
            {
                if (sources == null || sources.Count == 0) return;
                if (sources[0] == null)
                {
                    _logger.LogError("Первый источник графиков (sources[0]) равен null!");
                    return;
                }

                Vector2 min = _infoProvider.CurrentMin;
                Vector2 max = _infoProvider.CurrentMax;

                float cursorRealX = Mathf.Lerp(min.x, max.x, normalizedPos.x);
                float cursorRealY = Mathf.Lerp(min.y, max.y, normalizedPos.y);
                string xUnit = sources[0].XAxisLabel ?? "";
                string yUnit = sources[0].YAxisLabel ?? "";
                
                float closestSnappedX = cursorRealX;
                float closestSnappedY = min.y;
                
                float bestDiffY = float.MaxValue;
                float bestDiffX = float.MaxValue;

                string yValuesText = "";

                for (int i = 0; i < sources.Count; i++)
                {
                    var source = sources[i];
                    if (source == null) continue;

                    var points = source.GetPoints();
                    if (points == null || points.Count == 0) continue;

                    int closestIdx = 0;
                    float minDiffX = float.MaxValue;
                    for (int j = 0; j < points.Count; j++)
                    {
                        float diffX = Mathf.Abs(points[j].x - cursorRealX);
                        if (diffX < minDiffX)
                        {
                            minDiffX = diffX;
                            closestIdx = j;
                        }
                    }

                    Vector2 closestPoint = points[closestIdx];

                    if (minDiffX < bestDiffX)
                    {
                        bestDiffX = minDiffX;
                        closestSnappedX = closestPoint.x;
                    }
                    
                    float diffY = Mathf.Abs(closestPoint.y - cursorRealY);
                    if (diffY < bestDiffY)
                    {
                        bestDiffY = diffY;
                        closestSnappedY = closestPoint.y;
                    }

                    string sourceName = $"Эксперимент {i + 1}";
                    yValuesText += $"{sourceName}: {closestPoint.y:F2}\n";
                }
                
                string finalTooltipText = $"{xUnit}: {closestSnappedX:F2}\n" +
                                          $"{yUnit}: \n{yValuesText}";
                _view.Show(normalizedPos, finalTooltipText);

                Vector2 dotPosition = new Vector2(
                    Mathf.InverseLerp(min.x, max.x, closestSnappedX),
                    Mathf.InverseLerp(min.y, max.y, closestSnappedY) 
                );

                _view.UpdateDotPosition(dotPosition);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private void ShowLaboratoryMode(Vector2 normalizedPos, IReadOnlyList<IGraphDataSource> sources)
        {
            Vector2 min = _infoProvider.CurrentMin;
            Vector2 max = _infoProvider.CurrentMax;

            float minSqrDistance = float.MaxValue;
            Vector2 bestPoint = Vector2.zero;
            int bestSourceIndex = -1;

            for (int i = 0; i < sources.Count; i++)
            {
                var points = sources[i].GetPoints();
                foreach (var point in points)
                {
                    // Переводим реальную точку графика в нормализованные координаты (0..1)
                    // Это ВАЖНО, чтобы корректно считать дистанцию от мыши
                    Vector2 normalizedPoint = new Vector2(
                        Mathf.InverseLerp(min.x, max.x, point.x),
                        Mathf.InverseLerp(min.y, max.y, point.y)
                    );

                    float sqrDist = (normalizedPoint - normalizedPos).sqrMagnitude;
                    if (sqrDist < minSqrDistance)
                    {
                        minSqrDistance = sqrDist;
                        bestPoint = point;
                        bestSourceIndex = i;
                    }
                }
            }

            if (minSqrDistance > HoverThreshold * HoverThreshold)
            {
                _view.Hide();
                return;
            }

            var bestSource = sources[bestSourceIndex];
            string xUnit = bestSource.XAxisLabel;
            string yUnit = bestSource.YAxisLabel;


            string sourceName = $"Эксперимент {bestSourceIndex + 1}";

            string text = $"<b>{sourceName}</b>\n" +
                          $"X: {bestPoint.x:F2} {xUnit}\n" +
                          $"Y: {bestPoint.y:F2} {yUnit}";

            Vector2 snappedNormalizedPos = new Vector2(
                Mathf.InverseLerp(min.x, max.x, bestPoint.x),
                Mathf.InverseLerp(min.y, max.y, bestPoint.y)
            );

            _view.Show(normalizedPos, text);

            _view.UpdateDotPosition(snappedNormalizedPos);
        }

        private void HandlePointerExited() => _view.Hide();

        public void Dispose()
        {
            _view.OnPointerMoved -= HandlePointerMoved;
            _view.OnPointerExited -= HandlePointerExited;
        }
    }
}