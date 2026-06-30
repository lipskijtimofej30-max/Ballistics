using System;

namespace Game.Scripts.Core
{
    public interface IPhysicsIntegrator
    {
        void Step(Projectile projectile, Action onCollision);
    }
}