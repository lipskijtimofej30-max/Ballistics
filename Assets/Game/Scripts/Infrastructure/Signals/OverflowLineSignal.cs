namespace Assets.Game.Scripts.Infrastructure.Signals
{
    public class OverflowLineSignal
    {
        public bool IsOverflow { get; private set; }
        public OverflowLineSignal(bool isOverflow)
        {
            IsOverflow = isOverflow;
        }
    }
}
