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
        private readonly GraphView _view;
        private readonly GraphUseCase _useCase;
        private readonly GraphDataSourceFactory _factory;

        private List<SimulationRun> _runsForExperiment;
        private SimulationRun _currentRun;

        private bool _isMultiMode;

        [Inject]
        public GraphController(GraphView view, GraphDataSourceFactory factory, GraphUseCase useCase)
        {
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

                _view.RenderGraphs(sources);
            }
            else
            {
                if (_currentRun == null) return;

                var data = _factory.Create(graphType, _currentRun);
                _view.RenderGraph(data);
            }
        }

        public void Dispose()
        {
            _useCase.OnGraphTypeSelected -= HandleGraphTypeChanged;
        }
    }
}