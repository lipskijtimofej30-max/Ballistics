using System;
using Assets.Game.Scripts.Infrastructure.Signals;
using DefaultNamespace;
using Game.Scripts.Core;
using Game.Scripts.Infrastructure.Signals;
using Game.Scripts.Settings;
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.View.UseCase
{
    public class TrajectoryPreviewUseCase : IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly FastForwardSimulator _fastForwardSimulator;
        private readonly ProjectileFactory _projectileFactory;
        private readonly TrajectoryRenderer _trajectoryRenderer;
        private readonly Simulator _simulator;
        private readonly VisualizationSettings _visualizationSettings;
        private readonly IntegratorSettings _integratorSettings;

        [Inject]
        public TrajectoryPreviewUseCase(SignalBus signalBus, 
            FastForwardSimulator fastForwardSimulator,
            ProjectileFactory projectileFactory, 
            Simulator simulator,
            VisualizationSettings visualizationSettings,
            IntegratorSettings integratorSettings,
           [Inject(Id ="Preview")] TrajectoryRenderer trajectoryRenderer)
        {
            _signalBus = signalBus;
            _fastForwardSimulator = fastForwardSimulator;
            _projectileFactory = projectileFactory;
            _simulator = simulator;
            _visualizationSettings = visualizationSettings;
            _trajectoryRenderer = trajectoryRenderer;
            _integratorSettings = integratorSettings;
            
            _signalBus.Subscribe<ProjectileSettingsChangedSignal>(Refresh);
            _signalBus.Subscribe<SimulationSettingsChangedSignal>(Refresh);
            _signalBus.Subscribe<EnvironmentSettingsChangedSignal>(Refresh);
            _signalBus.Subscribe<ProjectileSpawnedSignal>(Refresh);
            _signalBus.Subscribe<IntegratorSettingsChangedSignal>(Refresh);
        }

        private void Refresh()
        {
            if(_simulator.CurrentBody == null || !_visualizationSettings.VisiblePreview) return;
            var state = _projectileFactory.CreateState();
            var run = _fastForwardSimulator.Run(state, _integratorSettings.IntegratorMethod, _integratorSettings.IntegrationStep);
            _trajectoryRenderer.DrawFull(run.Points);
        }
        
        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ProjectileSettingsChangedSignal>(Refresh);
            _signalBus.TryUnsubscribe<SimulationSettingsChangedSignal>(Refresh);
            _signalBus.TryUnsubscribe<EnvironmentSettingsChangedSignal>(Refresh);
            _signalBus.TryUnsubscribe<ProjectileSpawnedSignal>(Refresh);
            _signalBus.TryUnsubscribe<IntegratorSettingsChangedSignal>(Refresh);
        }
    }
}