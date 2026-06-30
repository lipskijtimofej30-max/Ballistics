using System;
using Game.Scripts.Core.Simulation;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class SemiImplicitEulerIntegrator : IPhysicsIntegrator
    {
    private ForceCalculator _forceCalculator;
    private SimulationRecorder _recorder;

    [Inject]
    private void Construct(ForceCalculator forceCalculator, SimulationRecorder recorder)
    {
        _forceCalculator = forceCalculator;
        _recorder = recorder;
    }

    public void Step(Projectile projectile, Action onCollision)
    {
        var force = _forceCalculator.CalculateTotalForce(projectile);
        var acceleration = force / projectile.Mass;

        projectile.Velocity += acceleration * Time.fixedDeltaTime;
        projectile.Position += projectile.Velocity * Time.fixedDeltaTime;

        projectile.transform.position = projectile.Position;
        
        _recorder.Record(
            projectile.Position,
            projectile.Velocity,
            acceleration,
            force);

        if (projectile.Position.y <= 0)
        {
            projectile.Position = new Vector3(
                projectile.Position.x,
                0,
                projectile.Position.z);
            onCollision?.Invoke();
        }
    }
    }
}