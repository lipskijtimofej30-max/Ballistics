using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Core.Graphics
{
    public interface IGraphInfoProvider
    {
        IReadOnlyList<IGraphDataSource> DataSources { get; }
        Vector2 CurrentMin { get; }
        Vector2 CurrentMax { get; }
        Vector2 GraphSize { get; }
    }
}