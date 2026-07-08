using System;
using Game.Scripts.Core.Simulation;
using Game.Scripts.Settings;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Core.Experiment
{
    public class ExperimentPlaybackController
    {
        private readonly IntegratorSettings _integratorSettings;
        
        private SimulationRun _currentRun;
        private float _elapsedTime;
        private int _cursorIndex;
        private bool _isPlaying;
        private bool _isPaused;
        
        public event Action<Vector3> PositionUpdated;
        public event Action PlaybackFinished;

        public bool IsPlaying => _isPlaying;

        [Inject]
        public ExperimentPlaybackController(IntegratorSettings integratorSettings) => _integratorSettings = integratorSettings;

        public void Play(SimulationRun run)
        {
            _elapsedTime = 0f;
            _cursorIndex = 0;
            _isPlaying = true;
            _isPaused = false;
            _currentRun = run;
        }

        public void Pause() => _isPaused = true;
        public void Resume() => _isPaused = false;

        public void Tick(float deltaTime)
        {
            if (!_isPlaying || _isPaused || _currentRun == null) return;
            if (_currentRun.Points.Count == 0) { Finish(); return; }

            _elapsedTime += deltaTime * _integratorSettings.TimeScale;

            while (_cursorIndex < _currentRun.Points.Count - 2 &&
                   _currentRun.Points[_cursorIndex + 1].Time <= _elapsedTime)
            {
                _cursorIndex++;
            }

            var pointA = _currentRun.Points[_cursorIndex];
            var pointB = _currentRun.Points[Mathf.Min(_cursorIndex + 1, _currentRun.Points.Count - 1)];

            float segmentDuration = pointB.Time - pointA.Time;
            float t = segmentDuration > 0f
                ? Mathf.Clamp01((_elapsedTime - pointA.Time) / segmentDuration)
                : 0f;

            Vector3 position = Vector3.Lerp(pointA.Position, pointB.Position, t);
            PositionUpdated?.Invoke(position);

            if (_elapsedTime >= _currentRun.Points[^1].Time)
                Finish();
        }

        private void Finish()
        {
            _isPlaying = false;
            PlaybackFinished?.Invoke();
        }
    }
}