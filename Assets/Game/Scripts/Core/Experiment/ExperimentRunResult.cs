using Game.Scripts.Core;
using Game.Scripts.Core.Simulation;
using Game.Scripts.Settings;

namespace Assets.Game.Scripts.Core.Experiment
{
    public class ExperimentRunResult
    {
        public int RunId { get; private set; }
        public ProjectileState InitialState { get; private set; }
        public SimulationSummary Summary { get; private set; }
        public SimulationRun Run {get; private set;}
        public ExperimentPreset Preset { get; private set; }

        public ExperimentRunResult(int runId, ProjectileState projectileState, SimulationSummary summary, SimulationRun run, ExperimentPreset preset)
        {
            RunId = runId;
            InitialState = projectileState;
            Summary = summary;
            Run = run;
            Preset = preset;
        }
    }
}
