using Zenject;

namespace Game.Scripts.Core
{
    public class RK2Integrator : IPhysicsIntegrator
    {
        private ForceCalculator _forceCalculator;

        [Inject]
        public RK2Integrator(ForceCalculator forceCalculator)
        {
            _forceCalculator = forceCalculator;
        }
        
        public IntegrationStepResult Step(ProjectileState projectile, float deltaTime)
        {
            var force1 = _forceCalculator.CalculateTotalForce(projectile);
            var acceleration1 = force1 / projectile.Mass;
 
            var midState = projectile.Clone();
            midState.Velocity = projectile.Velocity + acceleration1 * (deltaTime * 0.5f);
            midState.Position = projectile.Position + projectile.Velocity * (deltaTime * 0.5f);
            
            var force2 = _forceCalculator.CalculateTotalForce(midState);
            var acceleration2 = force2 / midState.Mass;
            
            projectile.Position += midState.Velocity * deltaTime;
            projectile.Velocity += acceleration2 * deltaTime;
 
            return new IntegrationStepResult(acceleration2, force2);
        }
    }
}