using System;
using Game.Scripts.Core.Simulation;

namespace Game.Scripts.Core
{
    public interface IPhysicsIntegrator
    {
        void Step(ProjectileState projectile, SimulationRun run, float deltaTime, Action onCollision);
    }
}