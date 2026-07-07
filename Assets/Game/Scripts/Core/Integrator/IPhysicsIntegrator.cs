using System;
using Game.Scripts.Core.Simulation;

namespace Game.Scripts.Core
{
    public interface IPhysicsIntegrator
    {
        IntegrationStepResult Step(ProjectileState projectile, float deltaTime);
    }
}