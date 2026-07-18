namespace Game.Scripts.Infrastructure.Signals
{
    public class ProjectileSpawnedSignal
    {
        public bool KeepPrevious { get; }

        public ProjectileSpawnedSignal(bool keepPrevious)
        {
            KeepPrevious = keepPrevious;
        }
    }
}