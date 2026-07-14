using Assets.Game.Scripts.Core.Experiment.Parameter;
using Game.Scripts.Core;
using Game.Scripts.Core.Simulation;
using System.Collections.Generic;
using Assets.Game.Scripts.Settings;
using Game.Scripts.Settings;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Core.Experiment
{
    public class ExperimentRunner
    {
        private readonly SimulationAnalyzer _analyzer;
        private readonly FastForwardSimulator _fastForwardSimulator;
        private readonly ProjectileFactory _projectileFactory;

        private readonly ProjectileSettings _projectileSettings;
        private readonly SimulationSettings _simulationSettings;
        private readonly EnvironmentSettings _environmentSettings;
        private readonly IntegratorSettings _integratorSettings;

        [Inject]
        public ExperimentRunner(SimulationAnalyzer analyzer, FastForwardSimulator fastForwardSimulator,
            ProjectileFactory projectileFactory,
            ProjectileSettings projectileSettings, SimulationSettings simulationSettings,
            EnvironmentSettings environmentSettings, IntegratorSettings integratorSettings)
        {
            _analyzer = analyzer;
            _fastForwardSimulator = fastForwardSimulator;
            _projectileFactory = projectileFactory;

            _projectileSettings = projectileSettings;
            _simulationSettings = simulationSettings;
            _environmentSettings = environmentSettings;
            _integratorSettings = integratorSettings;
        }
        
        public List<ExperimentRunResult> RunSeries(
            IExperimentParameter parameter,
            float minValue,
            float maxValue,
            float step,
            ExperimentPreset preset)
        {
            if (step <= 0f || minValue > maxValue)
                return new List<ExperimentRunResult>();

            var originalProjectile = _projectileSettings.Clone();
            var originalSimulation = _simulationSettings.Clone();
            var originalEnvironment = _environmentSettings.Clone();
            var originalIntegrator = _integratorSettings.Clone();

            ApplyPreset(preset);

            float originalParameterValue = parameter.GetValue();
            var results = new List<ExperimentRunResult>();
            int runId = 1;

            try
            {
                for (float value = minValue; value <= maxValue; value += step)
                {
                    parameter.SetValue(value);
                    var state = _projectileFactory.CreateState();
                    var run = _fastForwardSimulator.Run(
                        state,
                        _integratorSettings.IntegratorMethod,
                        _integratorSettings.IntegrationStep);
                    var summary = _analyzer.Analyze(run.Points);

                    results.Add(new ExperimentRunResult(runId, value, state.Clone(), summary, run, preset));
                    runId++;
                }
            }
            finally
            {
                parameter.SetValue(originalParameterValue);
                RestoreSettings(originalProjectile, originalSimulation, originalEnvironment, originalIntegrator);
            }

            return results;
        }

        private void ApplyPreset(ExperimentPreset p)
        {
            _projectileSettings.ShapeType = p.ShapeType;
            _projectileSettings.Density = p.Density;
            _projectileSettings.Size = p.Size;

            _simulationSettings.InitialSpeed = p.InitialSpeed;
            _simulationSettings.LaunchAngle = p.LaunchAngle;
            _simulationSettings.InitialPosition = new Vector3(0f, p.InitialHeight, 0f);

            _environmentSettings.Gravity = p.Gravity;
            _environmentSettings.WindVelocity = p.WindVelocity;
            _environmentSettings.AirDensity = p.AirDensity;
            _environmentSettings.AirResistanceEnabled = p.AirResistanceEnabled;

            _integratorSettings.IntegratorMethod = p.IntegratorMethod;
            _integratorSettings.IntegrationStep = p.IntegrationStep;
        }

        private void RestoreSettings(
            ProjectileSettings proj,
            SimulationSettings sim,
            EnvironmentSettings env,
            IntegratorSettings integ)
        {
            _projectileSettings.ShapeType = proj.ShapeType;
            _projectileSettings.Density = proj.Density;
            _projectileSettings.Size = proj.Size;

            _simulationSettings.InitialSpeed = sim.InitialSpeed;
            _simulationSettings.LaunchAngle = sim.LaunchAngle;
            _simulationSettings.InitialPosition = sim.InitialPosition;

            _environmentSettings.Gravity = env.Gravity;
            _environmentSettings.WindVelocity = env.WindVelocity;
            _environmentSettings.AirDensity = env.AirDensity;
            _environmentSettings.AirResistanceEnabled = env.AirResistanceEnabled;

            _integratorSettings.IntegratorMethod = integ.IntegratorMethod;
            _integratorSettings.IntegrationStep = integ.IntegrationStep;
        }
    }
}