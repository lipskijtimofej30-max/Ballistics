using Assets.Game.Scripts.Core.Experiment.Parameter;

namespace Assets.Game.Scripts.Settings
{
    public class ExperimentSettings
    {
        public int SelectedParameterIndex { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public float Step { get; set; }
    }
}
