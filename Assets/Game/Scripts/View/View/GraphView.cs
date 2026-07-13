using Assets.Game.Scripts.Core.Graphics;
using Game.Scripts.Core;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Scripts.View.View
{
    public class GraphView : MonoBehaviour
    {
        [field: SerializeField] public ParameterView CountLabelX { get; private set; }
        [field: SerializeField] public ParameterView CountLabelY { get; private set; }
        [field: SerializeField] public TMP_Dropdown Dropdown { get; private set; }
        [SerializeField] private GraphRenderer _renderer;
        [Header("Container")]
        [SerializeField] private GameObject _warningContainer;
        [SerializeField] private GameObject _graphContainer;
        
        private void Start()
        {    
            ToggleContainer(false);
        }

        public void ToggleContainer(bool toggle)
        {
            _graphContainer.SetActive(!toggle);
            _warningContainer.SetActive(toggle);
        }

        public void RenderGraph(IGraphDataSource dataSource)
        {
            _renderer.DrawSingleGraph(dataSource);
        }

        public void RenderGraphs(List<IGraphDataSource> sources)
        {
            _renderer.ClearAll();
            foreach (var source in sources)
            {
                _renderer.AddGraph(source);
            }
        }

        private void OnDestroy()
        {
            Dropdown.onValueChanged.RemoveAllListeners();
        }
    }
}