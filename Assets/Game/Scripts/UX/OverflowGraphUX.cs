using Assets.Game.Scripts.Infrastructure.Signals;
using Game.Scripts.View.View;
using System;
using Zenject;

namespace Assets.Game.Scripts.UX
{
    public class OverflowGraphUX : IInitializable, IDisposable
    {
        private readonly GraphView _graphView;
        private readonly SignalBus _signalBus;

        public OverflowGraphUX(GraphView graphView, SignalBus signalBus)
        {
            _graphView = graphView;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<OverflowLineSignal>(OnOverflow);
        }

        private void OnOverflow(OverflowLineSignal signal) => _graphView.ToggleContainer(signal.IsOverflow);
        public void Dispose()
        {
            _signalBus.TryUnsubscribe<OverflowLineSignal>(OnOverflow);
        }
    }
}
