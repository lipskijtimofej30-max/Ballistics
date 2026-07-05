using Game.Scripts.Core.Simulation;
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

        public SimulationRun Run(ProjectileState initialState, IntegratorMethod method, float dt)
        {
            var integrator = _integratorFactory.Create(method);
            var run = new SimulationRun();
            
            bool landed = false;
            int steps = 0;

            while (!landed || steps < MaxSteps)
            {
                integrator.Step(initialState, run, dt, () => landed = true);
                steps++;
            }
            return run;
        }
    }
}