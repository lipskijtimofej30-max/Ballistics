using System;
using Assets.Game.Scripts.Infrastructure.Signals;
using DefaultNamespace;
using Game.Scripts.Core;
using Game.Scripts.Infrastructure.Signals;
using Game.Scripts.View.View;
using UnityEngine;
using Zenject;

namespace Game.Scripts.View.UseCase
{
    public class TrajectoryPreviewUseCase : IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly FastForwardSimulator _fastForwardSimulator;
        private readonly ProjectileFactory _projectileFactory;
        private readonly TrajectoryRenderer _trajectoryRenderer;

        [Inject]
        public TrajectoryPreviewUseCase(SignalBus signalBus, 
            FastForwardSimulator fastForwardSimulator,
            ProjectileFactory projectileFactory, 
           [Inject(Id ="Preview")] TrajectoryRenderer trajectoryRenderer)
        {
            _signalBus = signalBus;
            _fastForwardSimulator = fastForwardSimulator;
            _projectileFactory = projectileFactory;
            _trajectoryRenderer = trajectoryRenderer;
            
            _signalBus.Subscribe<ProjectileSettingsChangedSignal>(Refresh);
            _signalBus.Subscribe<SimulationSettingsChangedSignal>(Refresh);
            _signalBus.Subscribe<EnvironmentSettingsChangedSignal>(Refresh);
        }

        private void Refresh()
        {
            var state = _projectileFactory.CreateState();
            var run = _fastForwardSimulator.Run(state, IntegratorMethod.SemiImplicitEuler, Time.fixedDeltaTime);
            _trajectoryRenderer.DrawFull(run.Points);
        }


        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ProjectileSettingsChangedSignal>(Refresh);
            _signalBus.TryUnsubscribe<SimulationSettingsChangedSignal>(Refresh);
            _signalBus.TryUnsubscribe<EnvironmentSettingsChangedSignal>(Refresh);
        }
    }
}