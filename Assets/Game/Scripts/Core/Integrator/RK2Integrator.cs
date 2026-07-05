using System;
using Game.Scripts.Core.Simulation;

namespace Game.Scripts.Core
{
    public class RK2Integrator : IPhysicsIntegrator
    {
        public void Step(ProjectileState projectile, SimulationRun run, float deltaTime, Action onCollision)
        {
            throw new NotImplementedException();
        }
    }
}