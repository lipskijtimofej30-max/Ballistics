using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Scripts.Core.Experiment
{
    public class ExperimentSession
    {
        private readonly List<ExperimentRunResult> _experimentRunResults = new();
        public IReadOnlyList<ExperimentRunResult> ExperimentRunResults => _experimentRunResults;

        public void Register(ExperimentRunResult result) => _experimentRunResults.Add(result);
        public void Unregister(ExperimentRunResult result) => _experimentRunResults.Remove(result);
        public void ClearAll() => _experimentRunResults.Clear();
    }
}
