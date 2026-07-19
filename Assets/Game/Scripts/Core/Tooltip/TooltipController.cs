using System;
using Game.Scripts.Infrastructure.Signals;
using Zenject;

namespace Game.Scripts.Core.Tooltip
{
    public class TooltipController : IDisposable
    {
        private readonly ITooltipView _view;
        private readonly SignalBus _signalBus;

        [Inject]
        public TooltipController(ITooltipView view, SignalBus signalBus)
        {
            _view = view;
            _signalBus = signalBus;
            _signalBus.Subscribe<ShowTooltipSignal>(OnShow);
            _signalBus.Subscribe<HideTooltipSignal>(OnHide);
        }

        private void OnShow(ShowTooltipSignal signal)
        {
            _view.Show(signal.Text, signal.ScreenPosition);
        }

        private void OnHide()
        {
            _view.Hide();
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ShowTooltipSignal>(OnShow);
            _signalBus.TryUnsubscribe<HideTooltipSignal>(OnHide);
        }
    }
}