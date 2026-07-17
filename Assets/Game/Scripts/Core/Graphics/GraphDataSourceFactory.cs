using System;
using Game.Scripts.Core.Simulation;

namespace Assets.Game.Scripts.Core.Graphics
{
    public class GraphDataSourceFactory
    {
        public IGraphDataSource Create(GraphType type, SimulationRun run)
        {
            return type switch
            {
                GraphType.Trajectory => new TrajectoryGraphDataSource(run),
                GraphType.SpeedTime => new SpeedTImeGraphDataSource(run),
                GraphType.RangeTime => new RangeTimeGraphDataSource(run),
                GraphType.AccelerationTime => new AccelerationTimeGraphDataSource(run),
                GraphType.HeightTime => new HeightTimeGraphData(run),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Неизвестный тип графика")
            };
        }
    }
}