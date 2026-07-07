using Zenject;

namespace Game.Scripts.Core
{
    public class VelocityVerletIntegrator : IPhysicsIntegrator
    {
        private readonly ForceCalculator _forceCalculator;

        [Inject]
        public VelocityVerletIntegrator(ForceCalculator forceCalculator) => _forceCalculator = forceCalculator;
        
        public IntegrationStepResult Step(ProjectileState projectile, float deltaTime)
        {
            var force1 = _forceCalculator.CalculateTotalForce(projectile);
            var acceleration1 = force1 / projectile.Mass;
            
            var state = projectile.Clone();
            state.Position += projectile.Velocity * deltaTime + 0.5f * acceleration1 * deltaTime * deltaTime;
            state.Velocity += 0.5f * acceleration1 * deltaTime;
            
            var force2 = _forceCalculator.CalculateTotalForce(state);
            var acceleration2 = force2 / projectile.Mass;
            
            projectile.Velocity = state.Velocity + 0.5f * acceleration2 * deltaTime;
            projectile.Position = state.Position;
            
            var finalForce = _forceCalculator.CalculateTotalForce(projectile);
            return new IntegrationStepResult(acceleration2, finalForce);
        }
    }
}