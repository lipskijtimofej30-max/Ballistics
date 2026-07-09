using Game.Scripts.Core;
using Game.Scripts.Core.Simulation;
using Game.Scripts.Settings;

namespace Assets.Game.Scripts.Core.Experiment
{
    public class ExperimentRunResult
    {
        public int RunId { get; private set; }
        public float ParameterValue { get; }
        public ProjectileState InitialState { get; private set; }
        public SimulationSummary Summary { get; private set; }
        public SimulationRun Run {get; private set;}
        public ExperimentPreset Preset { get; private set; }

        public ExperimentRunResult(
            int runId, 
            float parameterValue, // ДОБАВЛЕНО
            ProjectileState initialState, 
            SimulationSummary summary, 
            SimulationRun run, 
            ExperimentPreset preset)
        {
            RunId = runId;
            ParameterValue = parameterValue;
            InitialState = initialState;
            Summary = summary;
            Run = run;
            Preset = preset;
        }
    }
}
