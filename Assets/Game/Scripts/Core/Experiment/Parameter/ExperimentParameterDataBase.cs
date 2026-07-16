using System.Collections.Generic;
using Assets.Game.Scripts.Settings;
using Zenject;

namespace Assets.Game.Scripts.Core.Experiment.Parameter
{
    public class ExperimentParameterDataBase
    {
        private readonly List<IExperimentParameter> _parameters;
        private readonly ExperimentSettings _settings;
        
        public IReadOnlyList<IExperimentParameter> Parameters => _parameters;

        [Inject]
        public ExperimentParameterDataBase(List<IExperimentParameter> parameters, ExperimentSettings settings)
        {
            _parameters = parameters;
            _settings = settings;
        }

        public IExperimentParameter GetCurrentParameter() => _parameters[_settings.SelectedParameterIndex];
        public IExperimentParameter GetParameter(int index) => _parameters[index];
    }
}