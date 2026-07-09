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

        public bool IsPlaying => _isPlaying;
        public SimulationPoint CurrentPoint { get; private set; }
        public int CurrentRunIndex { get; private set; }
        
        [Inject]
        public ExperimentPlaybackController(IntegratorSettings integratorSettings) => _integratorSettings = integratorSettings;

        public void Play(int runIndex, SimulationRun run)
        {
            CurrentRunIndex = runIndex;
            
            _elapsedTime = 0f;
            _cursorIndex = 0;
            _isPlaying = true;
            _isPaused = false;
            _currentRun = run;
            
            CurrentPoint = run.Points.Count > 0 ? run.Points[0] : new SimulationPoint();
        }

        public void Pause() => _isPaused = true;
        public void Resume() => _isPaused = false;
        
        public void Stop()
        {
            _isPlaying = false;
            _isPaused = false;
            _currentRun = null;
        }

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

            CurrentPoint = new SimulationPoint
            {
                Time = Mathf.Lerp(pointA.Time, pointB.Time, t),
                Position = Vector3.Lerp(pointA.Position, pointB.Position, t),
                Velocity = Vector3.Lerp(pointA.Velocity, pointB.Velocity, t),
                Acceleration = Vector3.Lerp(pointA.Acceleration, pointB.Acceleration, t),
                TotalForce =  Vector3.Lerp(pointA.TotalForce, pointB.TotalForce, t)
            };

            PositionUpdated?.Invoke(CurrentPoint.Position);
            
            if (_elapsedTime >= _currentRun.Points[^1].Time)
                Finish();
        }

        private void Finish()
        {
            _isPlaying = false;
        }
    }
}