using Game.Scripts.Core;
using Game.Scripts.Core.Simulation;

namespace Assets.Game.Scripts.Core.Experiment
{
    public class ExperimentRunResult
    {
        public int RunId { get; private set; }
        public ProjectileState InitialState { get; private set; }
        public SimulationSummary Summary { get; private set; }

        public ExperimentRunResult(int runId, ProjectileState projectileState, SimulationSummary summary)
        {
            RunId = runId;
            InitialState = projectileState;
            Summary = summary;
        }
    }
}
