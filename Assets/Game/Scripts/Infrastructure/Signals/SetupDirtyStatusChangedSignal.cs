namespace Assets.Game.Scripts.Infrastructure.Signals
{
    public class SetupDirtyStatusChangedSignal
    {
        public bool IsDirty { get; }
        public SetupDirtyStatusChangedSignal(bool isDirty) => IsDirty = isDirty;
    }
}
