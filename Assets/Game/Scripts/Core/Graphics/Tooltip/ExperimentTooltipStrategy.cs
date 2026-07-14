using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Game.Scripts.Core.Graphics
{
    public class ExperimentTooltipStrategy : ITooltipStrategy
    {
        private readonly StringBuilder _sb = new StringBuilder(128);

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

                // Форматируем строку без аллокаций памяти
                _sb.Append("Эксперимент ").Append(i + 1).Append(": ").Append(closestPoint.y.ToString("F2"))
                    .AppendLine();
            }

            text = _sb.ToString();

            dotNormalizedPos = new Vector2(
                Mathf.InverseLerp(graphMin.x, graphMax.x, closestSnappedX),
                Mathf.InverseLerp(graphMin.y, graphMax.y, closestSnappedY)
            );

            return true;
        }
    }
}