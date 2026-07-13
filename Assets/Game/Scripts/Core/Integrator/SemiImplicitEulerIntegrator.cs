using Zenject;

namespace Game.Scripts.Core
{
    public class SemiImplicitEulerIntegrator : IPhysicsIntegrator
    {
        private ForceCalculator _forceCalculator;
 
        [Inject]
        private void Construct(ForceCalculator forceCalculator) => _forceCalculator = forceCalculator;
 
        public IntegrationStepResult Step(ProjectileState projectile, float deltaTime)
        {
            var force = _forceCalculator.CalculateTotalForce(projectile);
            var acceleration = force / projectile.Mass;
 
            // "Semi-implicit": скорость обновляется первой и используется
            // уже НОВАЯ скорость для обновления позиции.
            projectile.Velocity += acceleration * deltaTime;
            projectile.Position += projectile.Velocity * deltaTime;
 
            return new IntegrationStepResult(acceleration, force);
        }
    }
}