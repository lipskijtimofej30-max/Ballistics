using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Scripts.Core.Experiment.Parameter
{
    public interface IExperimentParameter
    {
        public string DisplayName { get; }
        public string Unit { get; }
        public float GetValue();
        public void SetValue(float value);
    }
}
