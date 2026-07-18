using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.Scripts.View.View
{
    public class SceneGridLabels : MonoBehaviour
    {
        [Header("Настройки")]
        [SerializeField] private TextMeshPro _labelPrefab;
        [SerializeField] private float _majorSpacing = 10f;
    
        [Header("Границы генерации")]
        [SerializeField] private float _maxDistanceX = 1000f;
        [SerializeField] private float _maxHeightY = 100f;
        [SerializeField] private float _offset = 1.5f;
        [SerializeField] private float _offsetZ = 4.5f;

        [Header("Оптимизация")]
        [SerializeField] private int _labelsPerFrame = 5; // Сколько текстов создаем за 1 кадр

        private void Start()
        {
            // Запускаем корутину вместо обычного метода
            StartCoroutine(GenerateLabelsRoutine());
        }

        private IEnumerator GenerateLabelsRoutine()
        {
            int count = 0;

            // 1. Рисуем отметки по оси X
            for (float x = _majorSpacing; x <= _maxDistanceX; x += _majorSpacing)
            {
                Vector3 position = new Vector3(x, -_offset, _offsetZ);
                TextMeshPro label = Instantiate(_labelPrefab, position, Quaternion.identity, transform);
                label.text = x.ToString();

                count++;
                // Если создали нужное количество в этом кадре — ждем следующего кадра
                if (count >= _labelsPerFrame)
                {
                    count = 0;
                    yield return null; 
                }
            }

            // 2. Рисуем отметки по оси Y
            for (float y = _majorSpacing; y <= _maxHeightY; y += _majorSpacing)
            {
                Vector3 position = new Vector3(-_offset, y, _offsetZ);
                TextMeshPro label = Instantiate(_labelPrefab, position, Quaternion.identity, transform);
                label.text = y.ToString();

                count++;
                if (count >= _labelsPerFrame)
                {
                    count = 0;
                    yield return null;
                }
            }
        }
    }
}