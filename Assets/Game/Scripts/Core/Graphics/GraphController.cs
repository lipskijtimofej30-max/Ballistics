using System;
using System.Collections.Generic;
using Assets.Game.Scripts.View.UseCase;
using Game.Scripts.Core.Simulation;
using Game.Scripts.View.View;
using Zenject;

namespace Assets.Game.Scripts.Core.Graphics
{
    public class GraphController : IDisposable
    {
        private readonly GraphLegendView _legendView;
        private readonly GraphView _view;
        private readonly GraphUseCase _useCase;
        private readonly GraphDataSourceFactory _factory;

        private List<SimulationRun> _runsForExperiment;
        private SimulationRun _currentRun;

        private bool _isMultiMode;

        [Inject]
        public GraphController(GraphLegendView legendView, GraphView view, GraphDataSourceFactory factory, GraphUseCase useCase)
        {
            _legendView = legendView;
            _view = view;
            _factory = factory;
            _useCase = useCase;

            _useCase.OnGraphTypeSelected += HandleGraphTypeChanged;
        }

        public void SetupSingleData(SimulationRun run)
        {
            _isMultiMode = false;
            _currentRun = run;
            HandleGraphTypeChanged((GraphType)_view.Dropdown.value);
        }

        public void SetupMultiData(List<SimulationRun> runs)
        {
            _isMultiMode = true;
            _runsForExperiment = runs;
            HandleGraphTypeChanged((GraphType)_view.Dropdown.value);
        }

        private void HandleGraphTypeChanged(GraphType graphType)
        {
            if (_isMultiMode)
            {
                if (_runsForExperiment == null || _runsForExperiment.Count == 0) return;

                var sources = new List<IGraphDataSource>();
                foreach (var run in _runsForExperiment)
                {
                    sources.Add(_factory.Create(graphType, run));
                }

                _view.RenderGraphs(sources, sources[0].DisplayName);
                _legendView.RendererLegend(sources);
            }
            else
            {
                if (_currentRun == null) return;

                var data = _factory.Create(graphType, _currentRun);
                _view.RenderGraph(data, data.DisplayName);
            }
        }

        public void Dispose()
        {
            _useCase.OnGraphTypeSelected -= HandleGraphTypeChanged;
        }
    }
}