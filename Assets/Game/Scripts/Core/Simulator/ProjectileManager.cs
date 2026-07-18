using Game.Scripts.Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class ProjectileManager
    {
        private readonly ProjectileFactory _projectileFactory;
        private readonly SignalBus _signalBus;

        public ProjectileBody CurrentBody { get; private set; }
        public ProjectileState CurrentState { get; private set; }
        public ProjectileBody PreviousBody { get; private set; }

        [Inject]
        public ProjectileManager(ProjectileFactory projectileFactory, SignalBus signalBus)
        {
            _projectileFactory = projectileFactory;
            _signalBus = signalBus;
        }

        /// <summary>
        /// Создаёт новый снаряд. Если keepPrevious == true, предыдущее тело сохраняется.
        /// </summary>
        public void Spawn(bool keepPrevious = false)
        {
            if (!keepPrevious)
            {
                // Полная очистка: удаляем и текущий, и предыдущий снаряд
                if (CurrentBody != null) GameObject.Destroy(CurrentBody.gameObject);
                if (PreviousBody != null) GameObject.Destroy(PreviousBody.gameObject);
                CurrentBody = null;
                PreviousBody = null;
                CurrentState = null;
            }
            else
            {
                // Сохраняем текущее тело как предыдущее
                if (PreviousBody != null) GameObject.Destroy(PreviousBody.gameObject);
                PreviousBody = CurrentBody;
                CurrentBody = null;
                CurrentState = null;
            }

            // Создаём новый снаряд
            CurrentBody = _projectileFactory.CreateBody();
            CurrentState = _projectileFactory.CreateState();
            CurrentBody.SyncTransform(CurrentState.Position);

            // Сообщаем визуализатору о новом снаряде, передавая флаг
            _signalBus.Fire(new ProjectileSpawnedSignal(keepPrevious));
        }

        /// <summary>
        /// Удаляет все снаряды и сбрасывает состояние.
        /// </summary>
        public void ClearAll()
        {
            if (CurrentBody != null) GameObject.Destroy(CurrentBody.gameObject);
            if (PreviousBody != null) GameObject.Destroy(PreviousBody.gameObject);

            CurrentBody = null;
            PreviousBody = null;
            CurrentState = null;
        }
    }
}