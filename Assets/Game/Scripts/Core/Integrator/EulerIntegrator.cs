using System;
using Game.Scripts.Core.Simulation;

namespace Game.Scripts.Core
{
    public class EulerIntegrator : IPhysicsIntegrator
    {
        public void Step(ProjectileState projectile, SimulationRun run, float deltaTime, Action onCollision)
        {
            throw new NotImplementedException();
        }
    }
}