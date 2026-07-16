using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Game.Scripts.Core.Experiment;
using Assets.Game.Scripts.Core.Experiment.Parameter;
using Game.Scripts.View.View;
using Zenject;

namespace Assets.Game.Scripts.Core.Graphics
{
    public class ExperimentGraphFilterController : IDisposable
    {
        private readonly ExperimentSession _session;
        private readonly GraphController _graphController;
        private readonly ExperimentParameterDataBase _parameters;
        private readonly ExperimentGraphFilterView _view;
        
        private readonly HashSet<int> _activeRunIds = new();

        [Inject]
        public ExperimentGraphFilterController(ExperimentSession session, GraphController graphController,
            ExperimentParameterDataBase parameters,  ExperimentGraphFilterView view)
        {
            _session = session;
            _graphController = graphController;
            _parameters = parameters;
            _view = view;
        }

        public void Initialize()
        {
            var results = _session.ExperimentRunResults;
            
            _activeRunIds.Clear();
            foreach (var result in results)
                _activeRunIds.Add(result.RunId);

            if (results.Count > 6)
            {
                string unit = _parameters.GetCurrentParameter().Unit;
                _view.BuildList(results, unit);
                _view.ToggleChanged += OnToggleChanged;
                _view.Show();
            }
            else
            {
                _view.Hide();
            }
            UpdateGraph();
        }

        private void OnToggleChanged(int runId, bool isActive)
        {
            if(isActive) _activeRunIds.Add(runId);
            else _activeRunIds.Remove(runId);

            UpdateGraph();
        }

        private void UpdateGraph()
        {
            var filterRuns = _session.ExperimentRunResults
                .Where(r => _activeRunIds.Contains(r.RunId))
                .Select(r => r.Run)
                .ToList();
            
            _graphController.ClearRuns();
            
            _graphController.SetupMultiData(filterRuns);
        }

        public void Dispose()
        {
            _view.ToggleChanged -= OnToggleChanged;
            _view.Clear();
            _view.Hide();
        }
    }
}