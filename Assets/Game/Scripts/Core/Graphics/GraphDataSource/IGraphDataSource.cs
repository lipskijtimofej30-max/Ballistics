using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Core.Graphics
{
    public interface IGraphDataSource
    {
        bool IsVisible { get; set; }
        string XAxisLabel { get; }
        string YAxisLabel { get; }
        string DisplayName { get; }
        
        Vector2 MinBound { get; }
        Vector2 MaxBound { get; }
        
        IReadOnlyList<Vector2> GetPoints();
    }
}