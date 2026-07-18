using System;
using Assets.Game.Scripts.Infrastructure.Signals;
using DefaultNamespace;
using Game.Scripts.Core;
using Game.Scripts.Infrastructure.Signals;
using Zenject;

namespace Game.Scripts.UX
{
    public class SetupDirtyTracker : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly Simulator _simulator;

        public bool IsDirty { get; private set; }
        public bool IsNewDirty { get; private set; }

        [Inject]
        public SetupDirtyTracker(SignalBus signalBus, Simulator simulator)
        {
            _signalBus = signalBus;
            _simulator = simulator;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ProjectileSettingsChangedSignal>(MarkDirty);
            _signalBus.Subscribe<SimulationSettingsChangedSignal>(MarkDirty);
            _signalBus.Subscribe<EnvironmentSettingsChangedSignal>(MarkDirty);
            _signalBus.Subscribe<IntegratorSettingsChangedSignal>(MarkDirty);
            
            _signalBus.Subscribe<ProjectileSettingsChangedSignal>(MarkNewDirty);
            _signalBus.Subscribe<SimulationSettingsChangedSignal>(MarkNewDirty);
            _signalBus.Subscribe<EnvironmentSettingsChangedSignal>(MarkNewDirty);
            _signalBus.Subscribe<IntegratorSettingsChangedSignal>(MarkNewDirty);

            _signalBus.Subscribe<CleanSetupRequestedSignal>(MarkClean);
            _signalBus.Subscribe<CleanSetupRequestedSignal>(MarkNewClean);
        }

        private void MarkDirty()
        {
            if (_simulator.CurrentBody == null || IsDirty) return;
            IsDirty = true;
            _signalBus.Fire(new SetupDirtyStatusChangedSignal(true));
        }
        private void MarkNewDirty()
        {
            if (_simulator.PreviousBody == null || IsNewDirty) return;
            IsNewDirty = true;
            _signalBus.Fire(new NewSetupDirtyStatusChangedSignal(true));
        }

        public void MarkClean()
        {
            IsDirty = false;
            _signalBus.Fire(new SetupDirtyStatusChangedSignal(false));
        }
        public void MarkNewClean()
        {
            IsNewDirty = false;
            _signalBus.Fire(new NewSetupDirtyStatusChangedSignal(false));
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ProjectileSettingsChangedSignal>(MarkDirty);
            _signalBus.TryUnsubscribe<SimulationSettingsChangedSignal>(MarkDirty);
            _signalBus.TryUnsubscribe<EnvironmentSettingsChangedSignal>(MarkDirty);
            _signalBus.TryUnsubscribe<IntegratorSettingsChangedSignal>(MarkDirty);
            
            _signalBus.TryUnsubscribe<ProjectileSettingsChangedSignal>(MarkNewDirty);
            _signalBus.TryUnsubscribe<SimulationSettingsChangedSignal>(MarkNewDirty);
            _signalBus.TryUnsubscribe<EnvironmentSettingsChangedSignal>(MarkNewDirty);
            _signalBus.TryUnsubscribe<IntegratorSettingsChangedSignal>(MarkNewDirty);
            
            _signalBus.TryUnsubscribe<CleanSetupRequestedSignal>(MarkClean);
            _signalBus.TryUnsubscribe<CleanSetupRequestedSignal>(MarkNewClean);
        }
    }
}