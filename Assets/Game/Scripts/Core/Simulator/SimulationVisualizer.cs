using System;
using Assets.Game.Scripts.View.View;
using Game.Scripts.Infrastructure.Signals;
using Game.Scripts.View.View;
using Zenject;

namespace Game.Scripts.Core
{
    public class SimulationVisualizer : IDisposable
    {
        private readonly TrajectoryPool _trajectoryPool;
        private readonly VectorRenderer _vectorRenderer;
        private readonly SignalBus _signalBus;

        private TrajectoryRenderer _currentTrajectory;
        private TrajectoryRenderer _previousTrajectory;
        private int _currentColorIndex;

        [Inject]
        public SimulationVisualizer(TrajectoryPool trajectoryPool, VectorRenderer vectorRenderer, SignalBus signalBus)
        {
            _trajectoryPool = trajectoryPool;
            _vectorRenderer = vectorRenderer;
            _signalBus = signalBus;

            _signalBus.Subscribe<ProjectileSpawnedSignal>(OnProjectileSpawned);
        }

        private void OnProjectileSpawned(ProjectileSpawnedSignal signal)
        {
            if (!signal.KeepPrevious)
            {
                // Полный сброс: удаляем все линии и начинаем с цвета 0
                ClearAll();
                _currentColorIndex = 0;
            }
            else
            {
                // Сдвиг: текущая траектория становится предыдущей
                if (_previousTrajectory != null)
                    _trajectoryPool.Release(_previousTrajectory);

                _previousTrajectory = _currentTrajectory;
                if (_previousTrajectory != null)
                {
                    // Цвет предыдущей = тот же индекс, но полупрозрачный
                    _previousTrajectory.SetColor(_currentColorIndex, 0.5f);
                }

                // Следующий цвет для новой траектории
                _currentColorIndex++;
            }

            // Создаём новую траекторию для текущего снаряда
            _currentTrajectory = _trajectoryPool.Rent();
            _currentTrajectory.SetVisible(true);
            _currentTrajectory.SetColor(_currentColorIndex, 1f);
        }

        public void UpdateVisuals(ProjectileState state)
        {
            _currentTrajectory?.AppendPoint(state.Position);
            _vectorRenderer.UpdateVectors(state);
        }

        public void FlushCurrentTrajectory()
        {
            _currentTrajectory?.FlushBuffer();
        }

        public void ClearAll()
        {
            if (_currentTrajectory != null)
            {
                _trajectoryPool.Release(_currentTrajectory);
                _currentTrajectory = null;
            }

            if (_previousTrajectory != null)
            {
                _trajectoryPool.Release(_previousTrajectory);
                _previousTrajectory = null;
            }

            _currentColorIndex = 0;
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ProjectileSpawnedSignal>(OnProjectileSpawned);
            ClearAll();
        }
    }
}