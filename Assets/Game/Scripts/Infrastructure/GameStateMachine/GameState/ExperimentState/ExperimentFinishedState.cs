using Game.Scripts.Infrastructure.GameStateMachine;
using System;
using System.Collections.Generic;
using Assets.Game.Scripts.Core.Experiment;
using Assets.Game.Scripts.Core.Experiment.Parameter;
using Game.Scripts.Core;
using Zenject;

namespace Assets.Game.Scripts.Infrastructure.GameStateMachine.ExperimentState
{
    public class ExperimentFinishedState : IGameState
    {
        private readonly ExperimentPlaybackSequencer _sequencer;
        private readonly ExperimentSession _session;
        private readonly TrajectoryPool _pool;

        [Inject]
        public ExperimentFinishedState(ExperimentPlaybackSequencer sequencer, ExperimentSession session, TrajectoryPool pool)
        {
            _sequencer = sequencer;
            _session = session;
            _pool = pool;
        }
        public void Enter()
        {
            
        }
        
        public void Tick()
        {
            
        }

        public void Exit()
        {
            _sequencer.StopSequence();
            _pool.ClearAll();
            _session.ClearAll();
        }
    }
}
