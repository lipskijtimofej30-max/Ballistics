using UnityEngine;

namespace Game.Scripts.Infrastructure.Logger
{
    public class Logger: ILogger
    {
        public bool IsEnabled { get; set; } = true;

        public void Log(string message)
        {
            if(!IsEnabled) return;
            Debug.Log(message);
        }

        public void LogError(string message)
        {
            if(!IsEnabled) return;
            Debug.LogError(message);
        }

        public void LogWarning(string message)
        {
            if(!IsEnabled) return;
            Debug.LogWarning(message);
        }
    }
}
