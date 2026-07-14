using System.Collections.Generic;
using Assets.Game.Scripts.Core.Graphics;
using UnityEngine;

namespace Game.Scripts.View.View
{
    public class GraphLegendView : MonoBehaviour
    {
        [SerializeField] private GraphLegendRow _rowPrefab;
        [SerializeField] private Transform _container;

        public void RendererLegend(List<IGraphDataSource> sources)
        {
            foreach (Transform child in _container)
                Destroy(child.gameObject);
            for (int i = 0; i < sources.Count; i++)
            {
                var row = Instantiate(_rowPrefab, _container);
                row.Initialize(GraphRenderer.LineColors[i], $"Эксп.{i+1}");
            }
        }
    }
}