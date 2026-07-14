using Assets.Game.Scripts.Core.Graphics;
using Game.Scripts.Core;
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
        [SerializeField] private TMP_Text _titleText;
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

        public void RenderGraph(IGraphDataSource dataSource, string title)
        {
            _renderer.DrawSingleGraph(dataSource);
            SetTitleText(title);
        }

        public void RenderGraphs(List<IGraphDataSource> sources, string title)
        {
            _renderer.ClearAll();
            foreach (var source in sources)
            {
                _renderer.AddGraph(source);
            }
            SetTitleText(title);
        }

        public void ClearAll()
        {
            _renderer.ClearAll();
        }
        
        private void SetTitleText(string title) => _titleText.text = title;

        private void OnDestroy()
        {
            Dropdown.onValueChanged.RemoveAllListeners();
        }
    }
}