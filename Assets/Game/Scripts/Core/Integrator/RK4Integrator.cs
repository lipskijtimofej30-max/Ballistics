using System;
using Game.Scripts.Core.Simulation;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class RK4Integrator : IPhysicsIntegrator
    {
        private struct Derivative
        {
            public Vector3 Velocity;
            public Vector3 Acceleration;
        }
        
        private ForceCalculator _forceCalculator;

        [Inject]
        public RK4Integrator(ForceCalculator forceCalculator) => _forceCalculator = forceCalculator;
        
        public IntegrationStepResult Step(ProjectileState projectile, float deltaTime)
        {
            var zero = new Derivative {Velocity = Vector3.zero, Acceleration = Vector3.zero};
            var k1 = Evaluate(projectile, deltaTime, zero);
            var k2 = Evaluate(projectile, deltaTime * 0.5f, k1);
            var k3 = Evaluate(projectile, deltaTime * 0.5f, k2);
            var k4 = Evaluate(projectile, deltaTime, k3);
            
            var avgVelocity = (k1.Velocity + 2f * (k2.Velocity + k3.Velocity) + k4.Velocity)*(1f/6f);
            var avgAcceleration = (k1.Acceleration + 2f * (k2.Acceleration + k3.Acceleration) + k4.Acceleration)*(1f/6f);
            
            projectile.Velocity += avgAcceleration * deltaTime;
            projectile.Position += avgVelocity * deltaTime;
            
            var finalForce = _forceCalculator.CalculateTotalForce(projectile);
            
            return new IntegrationStepResult(avgAcceleration, finalForce);
        }

        private Derivative Evaluate(ProjectileState baseState, float deltaTime, Derivative previous)
        {
            var state = baseState.Clone();
            state.Position += previous.Velocity * deltaTime;
            state.Velocity += previous.Acceleration * deltaTime;
            
            var force = _forceCalculator.CalculateTotalForce(state);
            var acceleration = force / state.Mass;
            
            return new Derivative {Velocity = state.Velocity, Acceleration = acceleration};
        }
    }
}