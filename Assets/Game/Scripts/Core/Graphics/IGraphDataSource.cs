using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IGraphDataSource
    {
        string XAxisLabel { get; }
        string YAxisLabel { get; }
        string DisplayName { get; }
        List<Vector2> GetPoints();
    }
}