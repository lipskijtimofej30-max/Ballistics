using Assets.Game.Scripts.Settings;

namespace Assets.Game.Scripts.Core.Experiment.Parameter
{
    public class LaunchAngleParameter : IExperimentParameter
    {
        private readonly SimulationSettings _simulationSettings;

        public LaunchAngleParameter(SimulationSettings simulationSettings) => _simulationSettings = simulationSettings;

        public string DisplayName => "Угол";

        public string Unit => "°";

        public float GetValue() => _simulationSettings.LaunchAngle;

        public void SetValue(float value) => _simulationSettings.LaunchAngle = value;
        public float MinRangeValue { get; } = 0f;
        public float MaxRangeValue { get; } = 90f;
    }
}
