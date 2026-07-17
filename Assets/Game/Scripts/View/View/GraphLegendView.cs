using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Game.Scripts.Core.Experiment;
using Assets.Game.Scripts.Core.Graphics;
using Assets.Game.Scripts.Settings;
using UnityEngine;
using Zenject;

namespace Game.Scripts.View.View
{
    public class GraphLegendView : MonoBehaviour
    {
        [SerializeField] private GraphLegendRow _rowPrefab;
        [SerializeField] private Transform _container;
        
        private GraphSettings  _settings;
        private List<GraphLegendRow> _legendPool = new List<GraphLegendRow>();
        private ExperimentSession _session;
        private ExperimentGraphFilterController _filterController;
        public event Action<int> OnLegendClicked;

        [Inject]
        private void Construct(GraphSettings settings, ExperimentSession session, ExperimentGraphFilterController filterController)
        {
            _settings = settings;
            _session = session;
            _filterController = filterController;
        }

        public void RendererLegend(List<IGraphDataSource> sources)
        {
            foreach (var row in _legendPool) row.gameObject.SetActive(false);

            int count = Mathf.Min(sources.Count, _settings.MaxLineCount, GraphRenderer.LineColors.Length); 
        
            var activeResults = (_session != null && _filterController != null)
                ? _session.ExperimentRunResults.Where(r => _filterController.ActiveRunIds.Contains(r.RunId)).ToList()
                : null;
    
            for (int i = 0; i < count; i++)
            {
                int runId = (activeResults != null && i < activeResults.Count) 
                    ? activeResults[i].RunId 
                    : (i + 1);

                var row = GetOrCreateRow(i);
                row.Initialize(i, GraphRenderer.LineColors[i], $"№ {runId}", sources[i].IsVisible, HandleRowClick);
            }
        }
    
        private void HandleRowClick(int index)
        {
            OnLegendClicked?.Invoke(index);
        }
    
        public void UpdateRowVisual(int index, bool isVisible)
        {
            if (index < _legendPool.Count)
                _legendPool[index].SetVisualState(isVisible);
        }
        
        private GraphLegendRow GetOrCreateRow(int index)
        {
            if (index < _legendPool.Count)
            {
                var row = _legendPool[index];
                row.gameObject.SetActive(true);
                return row;
            }
        
            var newRow = Instantiate(_rowPrefab, _container);
            _legendPool.Add(newRow);
            return newRow;
        }
        
        public void Show() => _container.gameObject.SetActive(true);
        public void Hide() => _container.gameObject.SetActive(false);
    }
}