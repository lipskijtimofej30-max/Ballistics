using Game.Scripts.Core.Simulation;
using Game.Scripts.Settings;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class FastForwardSimulator
    {
        private const int MaxSteps = 100_000;
        
        private readonly IntegratorFactory _integratorFactory;

        [Inject]
        public FastForwardSimulator(IntegratorFactory integratorFactory)
        {
            _integratorFactory = integratorFactory;
        }

        public SimulationRun Run(ProjectileState originalState, IntegratorMethod method, float dt)
        {
            var state = originalState.Clone();
            var integrator = _integratorFactory.Create(method);
            var stepper = new SimulationStepper(integrator);
            var run = new SimulationRun();
            
            run.AddPoint(state.Position, state.Velocity, Vector3.zero, Vector3.zero, 0f);
            
            bool landed = false;
            int steps = 0;

            while (!landed && steps < MaxSteps)
            {
                landed =  stepper.Step(state, run, dt);
                steps++;
            }
            return run;
        }
    }
}