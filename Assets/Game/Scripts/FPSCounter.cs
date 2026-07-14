using TMPro;
using UnityEngine;

namespace Game.Scripts
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _updateInterval = 0.1f;
    
        private float _elapsed;
        private int _frameCount;
        private float _currentFps;
    
        private float _minFps = float.MaxValue;
        private float _maxFps = float.MinValue;
        private float _sumFps;
        private int _measurements;
        private float _measureTimer;
        private const float MeasureWindow = 3f;

        private void Update()
        {
            _elapsed += Time.unscaledDeltaTime;
            _frameCount++;

            if (_elapsed >= _updateInterval)
            {
                _currentFps = _frameCount / _elapsed;
                _elapsed = 0f;
                _frameCount = 0;
            }

            _measureTimer += Time.unscaledDeltaTime;
            _sumFps += 1f / Time.unscaledDeltaTime;
            float instantFps = 1f / Time.unscaledDeltaTime;
            if (instantFps < _minFps) _minFps = instantFps;
            if (instantFps > _maxFps) _maxFps = instantFps;
            _measurements++;

            if (_measureTimer >= MeasureWindow)
            {
                float avg = _sumFps / _measurements;
                _text.text = $"FPS: {_currentFps:F1}\tAvg: {avg:F1} | Min: {_minFps:F1} | Max: {_maxFps:F1}";
                _minFps = float.MaxValue;
                _maxFps = float.MinValue;
                _sumFps = 0f;
                _measurements = 0;
                _measureTimer = 0f;
            }
        }
    }
}