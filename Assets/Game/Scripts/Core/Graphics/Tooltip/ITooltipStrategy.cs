using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Core.Graphics
{
    public interface ITooltipStrategy
    {
        bool TryGetTooltipData(
            Vector2 normalizedMousePos, 
            IReadOnlyList<IGraphDataSource> sources, 
            Vector2 graphMin, 
            Vector2 graphMax, 
            out string text, 
            out Vector2 dotNormalizedPos);
    }
}