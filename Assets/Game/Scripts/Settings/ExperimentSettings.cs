using Assets.Game.Scripts.Core.Experiment.Parameter;

namespace Assets.Game.Scripts.Settings
{
    public class ExperimentSettings
    {
        public int SelectedParameterIndex { get; set; } = 0;
        public float MinValue { get; set; } = 5f;
        public float MaxValue { get; set; } = 60f;
        public float Step { get; set; } = 10f;
        public float PauseBetweenRuns { get; set; } = 1f;
    }
}
