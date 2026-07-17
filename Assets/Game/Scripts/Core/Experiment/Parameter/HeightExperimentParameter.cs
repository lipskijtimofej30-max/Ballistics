using Assets.Game.Scripts.Settings;
using UnityEngine;

namespace Assets.Game.Scripts.Core.Experiment.Parameter
{
    public class HeightExperimentParameter : IExperimentParameter
    {
        private readonly SimulationSettings _settings;

        public HeightExperimentParameter(SimulationSettings settings)
        {
            _settings = settings;
        }

        public string DisplayName { get; } = "Высота";
        public string Unit { get; } = "м";
        public float GetValue() => _settings.InitialPosition.y;

        public void SetValue(float value)
        {
            _settings.InitialPosition =
                new Vector3(_settings.InitialPosition.x,
                    value,
                    _settings.InitialPosition.z);
        }

        public float MinRangeValue { get; } = 1f;
        public float MaxRangeValue { get; } = 15f;
    }
}