using System;
using System.Collections.Generic;
using Assets.Game.Scripts.Core.Graphics;
using TMPro;
using UnityEngine;

namespace Game.Scripts.View.View
{
    public class GraphView : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private GraphRenderer _renderer;

        public TMP_Dropdown Dropdown => _dropdown;
        
        public event Action<GraphType> OnGraphTypeSelected;

        private void Start()
        {
            _dropdown.ClearOptions();
            var options = new List<string>();
            foreach (GraphType type in Enum.GetValues(typeof(GraphType)))
                options.Add(type.GetDisplayName());
            _dropdown.AddOptions(options);
            
            _dropdown.onValueChanged.AddListener(index =>
                {
                    OnGraphTypeSelected?.Invoke((GraphType)index);   
                });
        }

        public void RenderGraph(IGraphDataSource dataSource)
        {
            _renderer.DrawGraph(dataSource);
        }

        private void OnDestroy()
        {
            _dropdown.onValueChanged.RemoveAllListeners();
        }
    }
}