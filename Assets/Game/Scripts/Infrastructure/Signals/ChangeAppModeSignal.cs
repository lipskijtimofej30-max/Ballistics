using Assets.Game.Scripts.Infrastructure.GameStateMachine;

namespace Game.Scripts.Infrastructure.Signals
{
    public class ChangeAppModeSignal
    {
        public AppMode NextAppMode { get; set; }
        public ChangeAppModeSignal(AppMode nextAppMode) => NextAppMode = nextAppMode;
    }
}