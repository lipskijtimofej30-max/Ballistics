using Assets.Game.Scripts.Core.Experiment.Parameter;
using Game.Scripts.Core;
using Game.Scripts.Core.Simulation;
using System.Collections.Generic;
using Zenject;

namespace Assets.Game.Scripts.Core.Experiment
{
    public class ExperimentRunner
    {
        private readonly SimulationAnalyzer _analyzer;
        private readonly FastForwardSimulator _fastForwardSimulator;
        private readonly ProjectileFactory _projectileFactory;

        [Inject]
        public ExperimentRunner(SimulationAnalyzer analyzer, FastForwardSimulator fastForwardSimulator,
            ProjectileFactory projectileFactory)
        {
            _analyzer = analyzer;
            _fastForwardSimulator = fastForwardSimulator;
            _projectileFactory = projectileFactory;
        }

        public List<ExperimentRunResult> RunSeries(IExperimentParameter parameter, float minValue, float maxValue, float step,
            IntegratorMethod method, float dt)
        {
            float originalValue = parameter.GetValue();
            var results = new List<ExperimentRunResult>();
            int runId = 1;

            for (float value = minValue; value <= maxValue; value += step)
            {
                parameter.SetValue(value);
                var state = _projectileFactory.CreateState();

                var run = _fastForwardSimulator.Run(state, method, dt);
                var summary = _analyzer.Analyze(run.Points);

                results.Add(new ExperimentRunResult(runId, state, summary));
                runId++;
            }
            parameter.SetValue(originalValue);
            return results;
        }
    }
}
