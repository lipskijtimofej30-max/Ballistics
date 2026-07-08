using Assets.Game.Scripts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
