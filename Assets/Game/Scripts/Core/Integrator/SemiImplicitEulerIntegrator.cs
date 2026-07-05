using System;
using Game.Scripts.Core.Simulation;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class SemiImplicitEulerIntegrator : IPhysicsIntegrator
    {
        private ForceCalculator _forceCalculator;

        [Inject]
        private void Construct(ForceCalculator forceCalculator)
        {
            _forceCalculator = forceCalculator;
        }

        public void Step(ProjectileState projectile, SimulationRun run, float deltaTime, Action onCollision)
        {
            var force = _forceCalculator.CalculateTotalForce(projectile);
            var acceleration = force / projectile.Mass;

            projectile.Velocity += acceleration * deltaTime;
            projectile.Position += projectile.Velocity * deltaTime;

            var landed = projectile.Position.y <= 0;
            if (landed)
                projectile.Position = new Vector3(projectile.Position.x, 0, projectile.Position.z);
            
            run.AddPoint(projectile.Position, projectile.Velocity, acceleration, force, deltaTime);

            if (landed)
                onCollision?.Invoke();
        }
    }
}