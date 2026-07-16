using Assets.Game.Scripts.Settings;

namespace Assets.Game.Scripts.Core.Experiment.Parameter
{
    public class InitialSpeedParameter : IExperimentParameter
    {
        private readonly SimulationSettings _simulationSettings;

        public InitialSpeedParameter(SimulationSettings simulationSettings) => _simulationSettings = simulationSettings;

        public string DisplayName => "Нач.скорость";

        public string Unit => "м/с";

        public float GetValue() => _simulationSettings.InitialSpeed;

        public void SetValue(float value) => _simulationSettings.InitialSpeed = value;
        
        public float MinRangeValue { get; } = 0f;
        public float MaxRangeValue { get; } = 50f;
    }
}