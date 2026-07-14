using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Core.Graphics
{
    public static class TooltipMathUtility
    {
        public static int FindClosestIndexByX(IReadOnlyList<Vector2> points, float targetX)
        {
            if (points == null || points.Count == 0) return -1;
        
            int left = 0;
            int right = points.Count - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                if (Mathf.Approximately(points[mid].x, targetX)) return mid;
            
                if (points[mid].x < targetX) 
                    left = mid + 1;
                else 
                    right = mid - 1;
            }

            if (left >= points.Count) return points.Count - 1;
            if (right < 0) return 0;

            return Mathf.Abs(points[left].x - targetX) < Mathf.Abs(points[right].x - targetX) ? left : right;
        }
    }
}