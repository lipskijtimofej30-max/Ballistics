using System;
using Assets.Game.Scripts.Infrastructure.Signals;
using DefaultNamespace;
using Game.Scripts.Core;
using Game.Scripts.Infrastructure.Signals;
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.UX
{
    public class SetupDirtyTracker : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly ToolbarView _toolbarView;
        private readonly Simulator  _simulator;
        
        public bool IsDirty { get; private set; }

        [Inject]
        public SetupDirtyTracker(SignalBus signalBus, ToolbarView toolbarView, Simulator simulator)
        {
            _signalBus = signalBus;
            _toolbarView = toolbarView;
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
            if(_simulator.CurrentBody == null) return;
            
            IsDirty = true;
            _toolbarView.CreateButtonLabel.text = "Обновить";
            _toolbarView.StartButton.interactable = false;
            _signalBus.Fire(new SetupDirtyStatusChangedSignal(isDirty: true));
        }

        public void MarkClean()
        {
            IsDirty = false;
            _toolbarView.CreateButtonLabel.text = "+ Создать";
            _toolbarView.StartButton.interactable = true;
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