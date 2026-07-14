using System.Collections.Generic;
using Assets.Game.Scripts.Core.Graphics;
using Assets.Game.Scripts.Infrastructure.Signals;
using Assets.Game.Scripts.Settings;
using Game.Scripts.Core;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Scripts.View.View
{
    public class GraphRenderer : MonoBehaviour, IGraphInfoProvider
    {
        public static Color[] LineColors =
            { new Color(0.309f, 0.639f, 1f), Color.red, Color.green, Color.yellow, Color.cyan, Color.blue };
        
        [Header("Axes Renderers")] 
        [SerializeField] private LineRenderer _xAxisLine;
        [SerializeField] private LineRenderer _yAxisLine;

        [Header("Labels")] 
        [SerializeField] private TMP_Text _labelPrefab;
        [SerializeField] private Transform _labelContainer;

        [Header("Lines Container")] 
        [SerializeField] private Transform _linesContainer;

        [Header("UI Offsets (Fix for overlapping)")] 
        [SerializeField] private Vector2 _xAxisLabelOffset = new Vector2(0f, -0.6f);
        [SerializeField] private Vector2 _yAxisLabelOffset = new Vector2(-1.5f, 0f);

        [Header("Graph Setup")]
        [SerializeField] private Vector2 _graphSize = new Vector2(10f, 10f);
        [SerializeField, Range(0f, 0.5f)] private float _padding = 0.05f;
        [SerializeField] private bool _forceZeroOrigin = true;
        
        [Header("Grid")]
        [SerializeField] private LineRenderer _gridLinePrefab;
        [SerializeField] private Transform _gridContainer;

        private List<IGraphDataSource> _dataSources = new List<IGraphDataSource>();
        private GraphLinePool _linePool;
        private GraphSettings _graphSettings;
        private SignalBus _signalBus;
        
        private List<TMP_Text> _labelPool = new List<TMP_Text>();
        private int _activeLabelsCount = 0;
        
        private List<LineRenderer> _gridPool = new List<LineRenderer>();
        private int _activeGridLinesCount = 0;
        
        public IReadOnlyList<IGraphDataSource> DataSources => _dataSources;
        public Vector2 CurrentMin { get; set; }
        public Vector2 CurrentMax { get; set; }
        public Vector2 GraphSize => _graphSize;

        [Inject]
        private void Construct(GraphLinePool linePool, GraphSettings graphSettings, SignalBus signalBus)
        {
            _linePool = linePool;
            _graphSettings = graphSettings;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _xAxisLine.useWorldSpace = false;
            _yAxisLine.useWorldSpace = false;

            _signalBus.Subscribe<GraphSettingsChangedSignal>(RedrawAxesAndLabels);
        }

        public void DrawSingleGraph(IGraphDataSource source)
        {
            ClearAll();
            AddGraph(source);
        }

        public void AddGraph(IGraphDataSource source)
        {
            if (source == null || source.GetPoints() == null || source.GetPoints().Count == 0) return;

            GraphLine newLine = _linePool.GetLine();

            if (newLine == null) return;

            _dataSources.Add(source);

            int activeCount = _linePool.GetActiveLines().Count;
            Color lineColor = LineColors[(activeCount - 1) % LineColors.Length];

            newLine.Initialize(source.GetPoints(), lineColor);

            newLine.transform.SetParent(_linesContainer);
            newLine.transform.localPosition = Vector3.zero;

            RedrawAll();
        }

        public void ClearAll()
        {
            _dataSources.Clear();
            _linePool.ReleaseAll();
            ReleaseLabels();
        }

        private void RedrawAll()
        {
            if (_dataSources.Count == 0) return;

            CalculateGlobalBounds(out Vector2 dataMin, out Vector2 dataMax);
            DrawAxesAndLabels(dataMin, dataMax);
            
            var activeLines = _linePool.GetActiveLines();

            for (int i = 0; i < _dataSources.Count; i++)
            {
                if (i < activeLines.Count)
                {
                    if (_dataSources[i].IsVisible)
                    {
                        activeLines[i].gameObject.SetActive(true);
                        activeLines[i].Draw(dataMin, dataMax, _graphSize);
                    }
                    else
                    {
                        activeLines[i].gameObject.SetActive(false);
                    }
                }
            }
        }

        private void RedrawAxesAndLabels()
        {
            if (_dataSources.Count == 0) return;

            CalculateGlobalBounds(out Vector2 dataMin, out Vector2 dataMax);
            DrawAxesAndLabels(dataMin, dataMax);
        }

        private void DrawAxesAndLabels(Vector2 dataMin, Vector2 dataMax)
        {
            CurrentMin = dataMin;
            CurrentMax = dataMax;
            
            DrawAxes();

            var firstSource = _dataSources[0];
            DrawGridAndLabels(dataMin, dataMax, firstSource.XAxisLabel, firstSource.YAxisLabel);
        }

        private void CalculateGlobalBounds(out Vector2 min, out Vector2 max)
        {
            min = new Vector2(float.MaxValue, float.MaxValue);
            max = new Vector2(float.MinValue, float.MinValue);
            bool hasVisible = false;
            
            foreach (var source in _dataSources)
            {
                if (!source.IsVisible) continue;
                hasVisible = true;
                if (source.MinBound.x < min.x) min.x = source.MinBound.x;
                if (source.MaxBound.x > max.x) max.x = source.MaxBound.x;
                if (source.MinBound.y < min.y) min.y = source.MinBound.y;
                if (source.MaxBound.y > max.y) max.y = source.MaxBound.y;
            }
            
            if (!hasVisible)
            {
                min = Vector2.zero;
                max = new Vector2(1f, 1f);
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
            ReleaseLabels();
            ReleaseGridLines();
            
            CreateLabel(xLabel, new Vector3(_graphSize.x * 0.5f, _xAxisLabelOffset.y, 0f), null, 6f);
            CreateLabel(yLabel, new Vector3(_yAxisLabelOffset.x, _graphSize.y * 0.5f, 0f), Quaternion.Euler(0, 0, 90), 7f);

            for (int i = 0; i <= _graphSettings.CountLabelX; i++)
            {
                float t = i / (float)_graphSettings.CountLabelX;
                float value = Mathf.Lerp(min.x, max.x, t);
                float xPos = t * _graphSize.x;
        
                CreateLabel(value.ToString("F2"), new Vector3(xPos, -0.3f, 0f));
        
                // Рисуем вертикальную линию сетки (кроме оси Y, где xPos = 0)
                if (i > 0)
                    CreateGridLine(new Vector3(xPos, 0, 0), new Vector3(xPos, _graphSize.y, 0));
            }

            // --- Горизонтальные линии сетки и метки Y ---
            for (int i = 0; i <= _graphSettings.CountLabelY; i++)
            {
                float t = i / (float)_graphSettings.CountLabelY;
                float value = Mathf.Lerp(min.y, max.y, t);
                float yPos = t * _graphSize.y;
        
                CreateLabel(value.ToString("F2"), new Vector3(-0.5f, yPos, 0f));
        
                // Рисуем горизонтальную линию сетки (кроме оси X, где yPos = 0)
                if (i > 0)
                    CreateGridLine(new Vector3(0, yPos, 0), new Vector3(_graphSize.x, yPos, 0));
            }
        }
        
        private void CreateGridLine(Vector3 start, Vector3 end)
        {
            LineRenderer line;
            if (_activeGridLinesCount < _gridPool.Count)
            {
                line = _gridPool[_activeGridLinesCount];
                line.gameObject.SetActive(true);
            }
            else
            {
                line = Instantiate(_gridLinePrefab, _gridContainer);
                line.useWorldSpace = false;
                _gridPool.Add(line);
            }

            _activeGridLinesCount++;
            line.positionCount = 2;
            line.SetPosition(0, start);
            line.SetPosition(1, end);
        }

        private void CreateLabel(string text, Vector3 localPos, Quaternion? rot = null, float fontSize = 5f)
        {
            TMP_Text label;
        
            // Берем из пула или создаем
            if (_activeLabelsCount < _labelPool.Count)
            {
                label = _labelPool[_activeLabelsCount];
                label.gameObject.SetActive(true);
            }
            else
            {
                label = Instantiate(_labelPrefab, _labelContainer);
                _labelPool.Add(label);
            }

            _activeLabelsCount++;
        
            label.transform.localPosition = localPos;
            label.transform.localRotation = rot ?? Quaternion.identity;
            label.text = text;
            label.fontSize = fontSize;
        }
        
        private void ReleaseLabels()
        {
            for (int i = 0; i < _activeLabelsCount; i++)
            {
                _labelPool[i].gameObject.SetActive(false);
            }
            _activeLabelsCount = 0;
        }
        
        public void ToggleGraphVisibility(int index)
        {
            if (index < 0 || index >= _dataSources.Count) return;
            
            _dataSources[index].IsVisible = !_dataSources[index].IsVisible;
        
            RedrawAll(); 
        }
        
        private void ReleaseGridLines()
        {
            for (int i = 0; i < _activeGridLinesCount; i++)
                _gridPool[i].gameObject.SetActive(false);
            _activeGridLinesCount = 0;
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<GraphSettingsChangedSignal>(RedrawAxesAndLabels);
        }
    }
}