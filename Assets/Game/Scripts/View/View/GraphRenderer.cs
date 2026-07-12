using System.Collections.Generic;
using Assets.Game.Scripts.Core.Graphics;
using Game.Scripts.Core;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Scripts.View.View
{
    public class GraphRenderer : MonoBehaviour
    {
        [Header("Axes Renderers")] 
        [SerializeField] private LineRenderer _xAxisLine;
        [SerializeField] private LineRenderer _yAxisLine;

        [Header("Labels")] 
        [SerializeField] private TMP_Text _labelPrefab;
        [SerializeField] private Transform _labelContainer;
        [SerializeField] private int _countLabelX = 6;
        [SerializeField] private int _countLabelY = 4;
        
        [Header("Lines Container")]
        [SerializeField] private Transform _linesContainer;
        
        [Header("UI Offsets (Fix for overlapping)")] 
        [SerializeField] private Vector2 _xAxisLabelOffset = new Vector2(0f, -0.6f);
        [SerializeField] private Vector2 _yAxisLabelOffset = new Vector2(-1.5f, 0f);

        [Header("Graph Setup")] 
        [SerializeField] private Vector2 _graphSize = new Vector2(10f, 10f);
        [SerializeField, Range(0f, 0.5f)] private float _padding = 0.05f;
        [SerializeField] private bool _forceZeroOrigin = true;
        
        private Color[] _lineColors = { new Color(0.309f, 0.639f, 1f), Color.red, Color.green, Color.yellow, Color.cyan };

        private List<IGraphDataSource> _dataSources = new List<IGraphDataSource>();
        private GraphLinePool _linePool;

        [Inject]
        private void Construct(GraphLinePool linePool)
        {
            _linePool = linePool;
        }

        private void Awake()
        {
            _xAxisLine.useWorldSpace = false;
            _yAxisLine.useWorldSpace = false;
        }

        public void DrawSingleGraph(IGraphDataSource source)
        {
            ClearAll();
            AddGraph(source);
        }

        public void AddGraph(IGraphDataSource source)
        {
            if (source == null || source.GetPoints() == null || source.GetPoints().Count == 0) return;

            _dataSources.Add(source);

            GraphLine newLine = _linePool.GetLine();

            int activeCount = _linePool.GetActiveLines().Count;
            Color lineColor = _lineColors[(activeCount - 1) % _lineColors.Length];

            newLine.Initialize(source.GetPoints(), lineColor);
            newLine.transform.SetParent(_linesContainer);
            newLine.transform.localPosition = Vector3.zero;

            RedrawAll();
        }

        public void ClearAll()
        {
            _dataSources.Clear();
            _linePool.ReleaseAll();
        }

        private void RedrawAll()
        {
            if (_dataSources.Count == 0) return;

            CalculateGlobalBounds(out Vector2 dataMin, out Vector2 dataMax);
            DrawAxes();

            var firstSource = _dataSources[0];
            DrawGridAndLabels(dataMin, dataMax, firstSource.XAxisLabel, firstSource.YAxisLabel);

            foreach (var line in _linePool.GetActiveLines())
            {
                line.Draw(dataMin, dataMax, _graphSize);
            }
        }

        private void CalculateGlobalBounds(out Vector2 min, out Vector2 max)
        {
            min = new Vector2(float.MaxValue, float.MaxValue);
            max = new Vector2(float.MinValue, float.MinValue);

            foreach (var source in _dataSources)
            {
                foreach (var p in source.GetPoints())
                {
                    if (p.x < min.x) min.x = p.x;
                    if (p.x > max.x) max.x = p.x;
                    if (p.y < min.y) min.y = p.y;
                    if (p.y > max.y) max.y = p.y;
                }
            }

            Vector2 range = new Vector2(max.x - min.x, max.y - min.y);
            if (range.x == 0) range.x = 1f;
            if (range.y == 0) range.y = 1f;

            max += range * _padding;
            min -= range * _padding;

            if (_forceZeroOrigin)
            {
                if (min.x > 0) min.x = 0;
                if (min.y > 0) min.y = 0;
            }
        }

        private void DrawAxes()
        {
            _xAxisLine.positionCount = 2;
            _xAxisLine.SetPosition(0, Vector3.zero);
            _xAxisLine.SetPosition(1, new Vector3(_graphSize.x, 0f, 0f));

            _yAxisLine.positionCount = 2;
            _yAxisLine.SetPosition(0, Vector3.zero);
            _yAxisLine.SetPosition(1, new Vector3(0f, _graphSize.y, 0f));
        }

        private void DrawGridAndLabels(Vector2 min, Vector2 max, string xLabel, string yLabel)
        {
            foreach (Transform child in _labelContainer) Destroy(child.gameObject);

            CreateLabel(xLabel, new Vector3(_graphSize.x * 0.5f, _xAxisLabelOffset.y, 0f), _labelContainer);
            CreateLabel(yLabel, new Vector3(_yAxisLabelOffset.x, _graphSize.y * 0.5f, 0f), _labelContainer,
                Quaternion.Euler(0, 0, 90));

            for (int i = 0; i <= _countLabelX; i++)
            {
                float t = i / (float)_countLabelX;
                float value = Mathf.Lerp(min.x, max.x, t);
                CreateLabel(value.ToString("F2"), new Vector3(t * _graphSize.x, -0.3f, 0f), _labelContainer);
            }

            for (int i = 0; i <= _countLabelY; i++)
            {
                float t = i / (float)_countLabelY;
                float value = Mathf.Lerp(min.y, max.y, t);
                CreateLabel(value.ToString("F2"), new Vector3(-0.5f, t * _graphSize.y, 0f), _labelContainer);
            }
        }

        private void CreateLabel(string text, Vector3 localPos, Transform parent, Quaternion? rot = null)
        {
            var label = Instantiate(_labelPrefab, parent);
            label.transform.localPosition = localPos;
            if (rot.HasValue) label.transform.rotation = rot.Value;
            label.text = text;
        }
    }
}