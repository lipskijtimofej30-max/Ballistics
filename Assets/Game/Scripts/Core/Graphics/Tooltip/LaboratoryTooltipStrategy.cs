using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Game.Scripts.Core.Graphics
{
    public class LaboratoryTooltipStrategy : ITooltipStrategy
    {
        private readonly StringBuilder _sb = new StringBuilder(128);
        private readonly float _hoverThresholdSqr;

        public LaboratoryTooltipStrategy(float hoverThreshold = 0.05f)
        {
            _hoverThresholdSqr = hoverThreshold * hoverThreshold;
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

            float minSqrDistance = float.MaxValue;
            Vector2 bestPoint = Vector2.zero;
            int bestSourceIndex = -1;

            float cursorRealX = Mathf.Lerp(graphMin.x, graphMax.x, normalizedMousePos.x);

            for (int i = 0; i < sources.Count; i++)
            {
                if (!sources[i].IsVisible) continue;
                
                var points = sources[i].GetPoints();
                if (points == null || points.Count == 0) continue;

                int centerIdx = TooltipMathUtility.FindClosestIndexByX(points, cursorRealX);
                if (centerIdx < 0) continue;
                
                int start = Mathf.Max(0, centerIdx - 5);
                int end = Mathf.Min(points.Count - 1, centerIdx + 5);

                for (int j = start; j <= end; j++)
                {
                    Vector2 point = points[j];
                    Vector2 normalizedPoint = new Vector2(
                        Mathf.InverseLerp(graphMin.x, graphMax.x, point.x),
                        Mathf.InverseLerp(graphMin.y, graphMax.y, point.y)
                    );

                    float sqrDist = (normalizedPoint - normalizedMousePos).sqrMagnitude;
                    if (sqrDist < minSqrDistance)
                    {
                        minSqrDistance = sqrDist;
                        bestPoint = point;
                        bestSourceIndex = i;
                    }
                }
            }

            if (minSqrDistance > _hoverThresholdSqr)
            {
                return false;
            }

            var bestSource = sources[bestSourceIndex];

            _sb.Clear();
            _sb.Append("<b>Эксперимент").AppendLine("</b>");
            _sb.Append(bestSource.XAxisLabel).Append(": ").Append(bestPoint.x.ToString("F2")).AppendLine();
            _sb.Append(bestSource.YAxisLabel).Append(": ").Append(bestPoint.y.ToString("F2"));

            text = _sb.ToString();

            dotNormalizedPos = new Vector2(
                Mathf.InverseLerp(graphMin.x, graphMax.x, bestPoint.x),
                Mathf.InverseLerp(graphMin.y, graphMax.y, bestPoint.y)
            );

            return true;
        }
    }
}