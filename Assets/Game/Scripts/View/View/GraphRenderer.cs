using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Game.Scripts.View.View
{
    public class GraphRenderer : MonoBehaviour
    {
        [Header("Line Renderers")] 
        [SerializeField] private LineRenderer _xAxisLine;
        [SerializeField] private LineRenderer _yAxisLine;
        [SerializeField] private LineRenderer _dataLine; // В будущем можно заменить на список/префаб для нескольких линий

        [Header("Graph Setup")]
        
        [Tooltip("Физический размер графика в локальных координатах Unity (ширина и высота)")]
        [SerializeField] private Vector2 _graphSize = new Vector2(10f, 10f);

        [Tooltip("Отступ от краев графика, чтобы линия не прилипала к осям (в долях, 0.1 = 10%)")]
        [SerializeField, Range(0f, 0.5f)]
        private float _padding = 0.05f;

        [Tooltip("Если включено, оси всегда будут начинаться с нуля (если минимальные значения больше нуля)")]
        [SerializeField]
        private bool _forceZeroOrigin = true;

        private void Awake()
        {
            // Убеждаемся, что линии рисуются в локальных координатах родителя (самого графика)
            _xAxisLine.useWorldSpace = false;
            _yAxisLine.useWorldSpace = false;
            _dataLine.useWorldSpace = false;
        }

        /// <summary>
        /// Основной метод для отрисовки графика из источника данных.
        /// </summary>
        public void DrawGraph(IGraphDataSource source)
        {
            if (source == null) return;

            List<Vector2> points = source.GetPoints();
            if (points == null || points.Count == 0)
            {
                Clear();
                return;
            }

            // 1. Вычисляем реальные границы данных (Min/Max) с учетом отступов
            CalculateBounds(points, out Vector2 dataMin, out Vector2 dataMax);

            // 2. Отрисовываем базовые оси X и Y
            DrawAxes();

            // 3. ЗАДЕЛ НА БУДУЩЕЕ: Здесь будет отрисовка сетки (Grid) и подписей
            DrawGridAndLabelsPlaceholder(dataMin, dataMax, source.XAxisLabel, source.YAxisLabel);

            // 4. Отрисовываем саму линию данных
            DrawDataLine(points, dataMin, dataMax);
        }

        public void Clear()
        {
            _dataLine.positionCount = 0;
        }

        private void CalculateBounds(List<Vector2> points, out Vector2 min, out Vector2 max)
        {
            min = new Vector2(float.MaxValue, float.MaxValue);
            max = new Vector2(float.MinValue, float.MinValue);

            // Ищем абсолютные минимумы и максимумы
            foreach (var p in points)
            {
                if (p.x < min.x) min.x = p.x;
                if (p.x > max.x) max.x = p.x;
                if (p.y < min.y) min.y = p.y;
                if (p.y > max.y) max.y = p.y;
            }

            // Вычисляем диапазон (разницу)
            Vector2 range = new Vector2(max.x - min.x, max.y - min.y);

            // Защита от плоских графиков (когда все значения равны, например y = 5)
            if (range.x == 0) range.x = 1f;
            if (range.y == 0) range.y = 1f;

            // Добавляем Padding (отступы), чтобы график не терся о края
            max += range * _padding;
            min -= range * _padding;

            // Если нужно, привязываем начало к нулю
            if (_forceZeroOrigin)
            {
                if (min.x > 0) min.x = 0;
                if (min.y > 0) min.y = 0;
            }
        }

        private void DrawAxes()
        {
            // Ось X: от (0,0) до (Ширина, 0)
            _xAxisLine.positionCount = 2;
            _xAxisLine.SetPosition(0, Vector3.zero);
            _xAxisLine.SetPosition(1, new Vector3(_graphSize.x, 0f, 0f));

            // Ось Y: от (0,0) до (0, Высота)
            _yAxisLine.positionCount = 2;
            _yAxisLine.SetPosition(0, Vector3.zero);
            _yAxisLine.SetPosition(1, new Vector3(0f, _graphSize.y, 0f));
        }

        private void DrawDataLine(List<Vector2> points, Vector2 dataMin, Vector2 dataMax)
        {
            _dataLine.positionCount = points.Count;

            var positions = new Vector3[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                float nx = Mathf.InverseLerp(dataMin.x, dataMax.x, points[i].x);
                float ny = Mathf.InverseLerp(dataMin.y, dataMax.y, points[i].y);
                positions[i] = new Vector3(nx * _graphSize.x, ny * _graphSize.y, 0f);
            }
            _dataLine.positionCount = points.Count;
            _dataLine.SetPositions(positions);
        }

        /// <summary>
        /// Заглушка для будущей реализации сетки (Tick marks) и текста осей
        /// </summary>
        private void DrawGridAndLabelsPlaceholder(Vector2 min, Vector2 max, string xLabel, string yLabel)
        {
            // В будущем здесь будет:
            // 1. Вычисление шага деления (например, с использованием Math.Log10 для "красивых" круглых чисел).
            // 2. Включение/Выключение дополнительных LineRenderer'ов для сетки (вертикальных и горизонтальных).
            // 3. Размещение TextMeshPro компонентов для подписей делений на основе координат.
        }
    }
}