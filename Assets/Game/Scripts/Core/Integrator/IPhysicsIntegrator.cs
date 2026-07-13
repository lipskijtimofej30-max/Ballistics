namespace Game.Scripts.Core
{
    public interface IPhysicsIntegrator
    {
        IntegrationStepResult Step(ProjectileState projectile, float deltaTime);
    }
}