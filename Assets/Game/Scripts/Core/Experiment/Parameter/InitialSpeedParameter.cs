using Assets.Game.Scripts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
