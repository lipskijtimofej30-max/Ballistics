using System;
using Game.Scripts.Core.Simulation;
using Zenject;

namespace Game.Scripts.Core
{
    public class EulerIntegrator : IPhysicsIntegrator
    {
        private ForceCalculator _forceCalculator;

        [Inject]
        public EulerIntegrator(ForceCalculator forceCalculator)
        {
            _forceCalculator = forceCalculator;
        }
        
        public IntegrationStepResult Step(ProjectileState projectile, float deltaTime)
        {
            var force = _forceCalculator.CalculateTotalForce(projectile);
            var acceleration = force / projectile.Mass;
 
            // "Euler": сначала позиция потом скорость
            var oldVelocity = projectile.Velocity;
            
            projectile.Position += oldVelocity * deltaTime;
            projectile.Velocity += acceleration * deltaTime;
 
            return new IntegrationStepResult(acceleration, force);
        }
    }
}