using Assets.Game.Scripts.Infrastructure.Signals;
using Game.Scripts.View.View;
using System;
using Assets.Game.Scripts.Core.Graphics;
using Zenject;

namespace Assets.Game.Scripts.UX
{
    public class OverflowGraphUX : IInitializable, IDisposable
    {
        private readonly GraphView _graphView;
        private readonly GraphTooltipController _tooltipController;
        private readonly SignalBus _signalBus;

        public OverflowGraphUX(GraphView graphView, GraphTooltipController tooltipController, SignalBus signalBus)
        {
            _graphView = graphView;
            _tooltipController = tooltipController;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<OverflowLineSignal>(OnOverflow);
        }

        private void OnOverflow(OverflowLineSignal signal)
        { 
            _graphView.ToggleContainer(signal.IsOverflow);
            _tooltipController.Enabled = !signal.IsOverflow;
        } 
        public void Dispose()
        {
            _signalBus.TryUnsubscribe<OverflowLineSignal>(OnOverflow);
        }
    }
}
