using Assets.Game.Scripts.Infrastructure.Signals;
using Assets.Game.Scripts.Settings;
using Game.Scripts.View.View;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Game.Scripts.Core
{
    public class GraphLinePool
    {
        private readonly GraphSettings _graphSettings;
        private readonly Infrastructure.Logger.ILogger _logger;
        private readonly GraphLine _linePrefab;
        private readonly SignalBus _signalBus;
        private Queue<GraphLine> _pool = new Queue<GraphLine>();
        private List<GraphLine> _activeLines = new List<GraphLine>();

        [Inject]
        public GraphLinePool(GraphLine linePrefab, GraphSettings graphSettings, Infrastructure.Logger.ILogger logger, SignalBus signalBus)
        {
            _linePrefab = linePrefab;
            _graphSettings = graphSettings;
            _logger = logger;
            _signalBus = signalBus;
        }

        public GraphLine GetLine()
        {
            if(_activeLines.Count >= _graphSettings.MaxLineCount)
            {
                _logger.LogWarning("Active lines > max count");
                _signalBus.Fire(new OverflowLineSignal(true));
                return null;
            }

            GraphLine line;

            if (_pool.Count > 0)
            {
                line = _pool.Dequeue();
                line.gameObject.SetActive(true);
            }
            else
            {
                line = GameObject.Instantiate(_linePrefab);
            }
        
            _activeLines.Add(line);
            _signalBus.Fire(new OverflowLineSignal(false));
            return line;
        }

        public void ReleaseAll()
        {
            foreach (var line in _activeLines)
            {
                line.gameObject.SetActive(false);
                line.transform.SetParent(null);
                _pool.Enqueue(line);
            }
            _activeLines.Clear();
        }

        public List<GraphLine> GetActiveLines()
        {
            return _activeLines;
        }
    }
}