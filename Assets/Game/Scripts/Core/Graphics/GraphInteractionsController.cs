using System;
using Game.Scripts.View.View;
using Zenject;

namespace Assets.Game.Scripts.Core.Graphics
{
    public class GraphInteractionsController : IInitializable, IDisposable
    {
        private readonly GraphLegendView _legendView;
        private readonly GraphRenderer _graphRenderer;

        [Inject]
        public GraphInteractionsController(GraphLegendView legendView, GraphRenderer graphRenderer)
        {
            _legendView = legendView;
            _graphRenderer = graphRenderer;
        }

        public void Initialize()
        {
            _legendView.OnLegendClicked += HandleLegendClicked;
        }

        private void HandleLegendClicked(int index)
        {
            _graphRenderer.ToggleGraphVisibility(index);

            var source = _graphRenderer.DataSources[index];
            bool isVisible = source.IsVisible;

            _legendView.UpdateRowVisual(index, isVisible);
        }

        public void Dispose()
        {
            _legendView.OnLegendClicked -= HandleLegendClicked;
        }
    }
}