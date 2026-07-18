namespace Game.Scripts.Infrastructure.Signals
{
    public class NewSetupDirtyStatusChangedSignal
    {
        public bool IsNewDirty { get; }

        public NewSetupDirtyStatusChangedSignal(bool isNewDirty) => IsNewDirty = isNewDirty;
    }
}