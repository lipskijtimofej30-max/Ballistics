using Game.Scripts.Core.Simulation;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class SimulationStepper
    {
        private readonly IPhysicsIntegrator _integrator;
 
        public SimulationStepper(IPhysicsIntegrator integrator)
        {
            _integrator = integrator;
        }
 
        public bool Step(ProjectileState projectile, SimulationRun run, float deltaTime)
        {
            var result = _integrator.Step(projectile, deltaTime);

            projectile.Acceleration = result.Acceleration;
            projectile.TotalForce = result.TotalForce;
 
            bool landed = projectile.Position.y <= 0f;
            if (landed)
                projectile.Position = new Vector3(projectile.Position.x, 0f, projectile.Position.z);
 
            run.AddPoint(projectile.Position, projectile.Velocity, result.Acceleration, result.TotalForce, deltaTime);
 
            return landed;
        }
    }
}