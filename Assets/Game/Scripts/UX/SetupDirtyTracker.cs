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

            _signalBus.Subscribe<CleanSetupRequestedSignal>(MarkClean);
        }

        private void MarkDirty()
        {
            // Если симуляции нет или уже статус грязный - ничего не делаем
            if (_simulator.CurrentBody == null || IsDirty) return;

            IsDirty = true;

            // Просто отправляем сигнал, ToolbarView сам обновит свои кнопки!
            _signalBus.Fire(new SetupDirtyStatusChangedSignal(isDirty: true));
        }

        public void MarkClean()
        {
            IsDirty = false;

            // Просто отправляем сигнал
            _signalBus.Fire(new SetupDirtyStatusChangedSignal(isDirty: false));
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ProjectileSettingsChangedSignal>(MarkDirty);
            _signalBus.TryUnsubscribe<SimulationSettingsChangedSignal>(MarkDirty);
            _signalBus.TryUnsubscribe<EnvironmentSettingsChangedSignal>(MarkDirty);
            _signalBus.TryUnsubscribe<IntegratorSettingsChangedSignal>(MarkDirty);
            _signalBus.TryUnsubscribe<CleanSetupRequestedSignal>(MarkClean);
        }
    }
}