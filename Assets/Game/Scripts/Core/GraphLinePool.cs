using System.Collections.Generic;
using Game.Scripts.View.View;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class GraphLinePool
    {
        private GraphLine _linePrefab;
        private Queue<GraphLine> _pool = new Queue<GraphLine>();
        private List<GraphLine> _activeLines = new List<GraphLine>();

        [Inject]
        public GraphLinePool(GraphLine linePrefab)
        {
            _linePrefab = linePrefab;
        }

        public GraphLine GetLine()
        {
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