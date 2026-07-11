using System;
using Game.Scripts.Core.Simulation;
using Game.Scripts.View.View;
using Zenject;

namespace Assets.Game.Scripts.Core.Graphics
{
    public class GraphController : IDisposable
    {
        private readonly GraphView _view;
        private readonly GraphDataSourceFactory _factory;
        
        private SimulationRun _currentRun;

        [Inject]
        public GraphController(GraphView view, GraphDataSourceFactory factory)
        {
            _view = view;
            _factory = factory;

            _view.OnGraphTypeSelected += HandleGraphTypeChanged;
        }

        public void SetupData(SimulationRun run)
        {
            _currentRun = run;
            HandleGraphTypeChanged((GraphType)_view.Dropdown.value);
        }

        private void HandleGraphTypeChanged(GraphType graphType)
        {
            if(_currentRun == null) return;
            
            var data = _factory.Create(graphType, _currentRun);
            _view.RenderGraph(data);
        }
        
        public void Dispose()
        {
            _view.OnGraphTypeSelected -= HandleGraphTypeChanged;
        }
    }
}