namespace Game.Scripts.Infrastructure.Logger
{
    public interface ILogger
    {
        bool IsEnabled { get; set; }
        void Log(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}

