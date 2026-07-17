using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Game.Scripts.Core.Experiment;
using UnityEngine;

namespace Assets.Game.Scripts.Core.Graphics
{
    public class ExperimentTooltipStrategy : ITooltipStrategy
    {
        private readonly StringBuilder _sb = new StringBuilder(128);
        private readonly ExperimentSession _session;
        private readonly ExperimentGraphFilterController _filterController;

        private readonly Dictionary<IGraphDataSource, int> _runIdCache = new();

        public ExperimentTooltipStrategy(ExperimentSession session, ExperimentGraphFilterController filterController)
        {
            _session = session;
            _filterController = filterController;
        }

        public bool TryGetTooltipData(
            Vector2 normalizedMousePos,
            IReadOnlyList<IGraphDataSource> sources,
            Vector2 graphMin,
            Vector2 graphMax,
            out string text,
            out Vector2 dotNormalizedPos)
        {
            text = string.Empty;
            dotNormalizedPos = Vector2.zero;

            if (sources == null || sources.Count == 0) return false;

            if (!IsCacheValid(sources))
                RebuildCache(sources);

            _sb.Clear();

            float cursorRealX = Mathf.Lerp(graphMin.x, graphMax.x, normalizedMousePos.x);
            float cursorRealY = Mathf.Lerp(graphMin.y, graphMax.y, normalizedMousePos.y);

            string xUnit = sources[0].XAxisLabel ?? "";
            string yUnit = sources[0].YAxisLabel ?? "";

            float closestSnappedX = cursorRealX;
            float closestSnappedY = graphMin.y;

            float bestDiffY = float.MaxValue;
            float bestDiffX = float.MaxValue;

            _sb.Append(xUnit).Append(": ").Append(cursorRealX.ToString("F2")).AppendLine();
            _sb.Append(yUnit).AppendLine(":");

            for (int i = 0; i < sources.Count; i++)
            {
                var source = sources[i];
                if (!sources[i].IsVisible) continue;

                var points = sources[i].GetPoints();
                if (points == null || points.Count == 0) continue;

                int closestIdx = TooltipMathUtility.FindClosestIndexByX(points, cursorRealX);
                if (closestIdx < 0) continue;

                Vector2 closestPoint = points[closestIdx];

                float diffX = Mathf.Abs(closestPoint.x - cursorRealX);
                if (diffX < bestDiffX)
                {
                    bestDiffX = diffX;
                    closestSnappedX = closestPoint.x;
                }

                float diffY = Mathf.Abs(closestPoint.y - cursorRealY);
                if (diffY < bestDiffY)
                {
                    bestDiffY = diffY;
                    closestSnappedY = closestPoint.y;
                }

                int runId = _runIdCache.TryGetValue(source, out int cashedId) ? cashedId : (i + 1);

                _sb.Append("Эксперимент ").Append(runId).Append(": ").Append(closestPoint.y.ToString("F2"))
                    .AppendLine();
            }

            text = _sb.ToString();

            dotNormalizedPos = new Vector2(
                Mathf.InverseLerp(graphMin.x, graphMax.x, closestSnappedX),
                Mathf.InverseLerp(graphMin.y, graphMax.y, closestSnappedY)
            );

            return true;
        }

        private bool IsCacheValid(IReadOnlyList<IGraphDataSource> sources)
        {
            if (_runIdCache.Count != sources.Count) return false;

            for (int i = 0; i < sources.Count; i++)
            {
                if (!_runIdCache.ContainsKey(sources[i])) return false;
            }

            return true;
        }

        private void RebuildCache(IReadOnlyList<IGraphDataSource> sources)
        {
            _runIdCache.Clear();
            if (_session == null) return;

            var activeResults = _session.ExperimentRunResults
                .Where(r => _filterController.ActiveRunIds.Contains(r.RunId))
                .ToList();

            for (int i = 0; i < sources.Count && i < activeResults.Count; i++)
            {
                _runIdCache[sources[i]] = activeResults[i].RunId;
            }
        }
    }
}