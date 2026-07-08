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
        private readonly VisualizationSettings _visualizationSettings;
        private readonly IntegratorSettings _integratorSettings;

        [Inject]
        public ExperimentRunner(SimulationAnalyzer analyzer, FastForwardSimulator fastForwardSimulator,
            ProjectileFactory projectileFactory,
            ProjectileSettings projectileSettings, SimulationSettings simulationSettings,
            EnvironmentSettings environmentSettings, VisualizationSettings visualizationSettings,
            IntegratorSettings integratorSettings)
        {
            _analyzer = analyzer;
            _fastForwardSimulator = fastForwardSimulator;
            _projectileFactory = projectileFactory;

            _projectileSettings = projectileSettings;
            _simulationSettings = simulationSettings;
            _environmentSettings = environmentSettings;
            _visualizationSettings = visualizationSettings;
            _integratorSettings = integratorSettings;
        }

        /// <summary>
        /// Запускает серию прогонов, временно заменяя все настройки на значения из пресета.
        /// </summary>
        /// <param name="parameter">Варьируемый параметр (читает/пишет в реальные настройки, которые мы подменили).</param>
        /// <param name="minValue">Минимальное значение варьируемого параметра.</param>
        /// <param name="maxValue">Максимальное значение.</param>
        /// <param name="step">Шаг изменения.</param>
        /// <param name="preset">Пресет со значениями всех остальных параметров.</param>
        /// <returns>Список результатов ExperimentRunResult для каждого шага.</returns>
        public List<ExperimentRunResult> RunSeries(
            IExperimentParameter parameter,
            float minValue,
            float maxValue,
            float step,
            ExperimentPreset preset)
        {
            if (step <= 0f || minValue > maxValue)
                return new List<ExperimentRunResult>();

            // 1. Сохраняем оригинальные настройки
            var originalProjectile = _projectileSettings.Clone();
            var originalSimulation = _simulationSettings.Clone();
            var originalEnvironment = _environmentSettings.Clone();
            var originalIntegrator = _integratorSettings.Clone();

            // 2. Применяем пресет
            ApplyPreset(preset);

            // 3. Сохраняем исходное значение варьируемого параметра (уже из подменённых настроек)
            float originalParameterValue = parameter.GetValue();
            var results = new List<ExperimentRunResult>();
            int runId = 1;

            try
            {
                for (float value = minValue; value <= maxValue; value += step)
                {
                    parameter.SetValue(value);
                    var state = _projectileFactory.CreateState(); // фабрика видит подменённые настройки
                    var run = _fastForwardSimulator.Run(
                        state,
                        _integratorSettings.IntegratorMethod,
                        _integratorSettings.IntegrationStep);
                    var summary = _analyzer.Analyze(run.Points);

                    results.Add(new ExperimentRunResult(runId, state.Clone(), summary, run));
                    runId++;
                }
            }
            finally
            {
                // 4. Восстанавливаем оригинальные настройки в любом случае
                RestoreSettings(originalProjectile, originalSimulation, originalEnvironment, originalIntegrator);
                parameter.SetValue(originalParameterValue);
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