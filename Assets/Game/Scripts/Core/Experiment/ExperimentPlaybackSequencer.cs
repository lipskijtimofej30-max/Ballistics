using System;
using System.Collections;
using Assets.Game.Scripts.Infrastructure.GameStateMachine;
using DefaultNamespace;
using Game.Scripts.Core;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Core.Experiment
{
    public class ExperimentPlaybackSequencer : MonoBehaviour
    {
        private const float PauseBetweenRuns = 0.3f;

        private ExperimentSession _session;
        private ExperimentPlaybackController _playbackController;
        private TrajectoryPool _trajectoryPool;
        private SignalBus _signalBus;

        private Coroutine _routine;
        private Action _currentFinishedCallback;

        [Inject]
        private void Construct(
            ExperimentSession session,
            ExperimentPlaybackController playbackController,
            TrajectoryPool trajectoryPool,
            SignalBus signalBus)
        {
            _session = session;
            _playbackController = playbackController;
            _trajectoryPool = trajectoryPool;
            _signalBus = signalBus;
        }

        public void StartSequence()
        {
            StopSequence();
            _routine = StartCoroutine(PlaySeriesRoutine());
        }

        public void StopSequence()
        {
            if (_routine == null) return;
            StopCoroutine(_routine);
            _routine = null;

            _playbackController.Pause();
            
            if (_currentFinishedCallback != null)
            {
                _playbackController.PlaybackFinished -= _currentFinishedCallback;
                _currentFinishedCallback = null;
            }
        }

        private IEnumerator PlaySeriesRoutine()
        {
            foreach (var result in _session.ExperimentRunResults)
            {
                _playbackController.Play(result.Run);

                var renderer = _trajectoryPool.Rent();
                renderer.DrawFull(result.Run.Points);
                renderer.SetVisible(true);
                renderer.SetColor(result.RunId);
                
                yield return new WaitUntil(() => !_playbackController.IsPlaying);

                _playbackController.PlaybackFinished -= _currentFinishedCallback;
                yield return new WaitForSeconds(PauseBetweenRuns);
            }

            _signalBus.Fire(new ChangeStateSignal<ExperimentStateType>(ExperimentStateType.Finished));
        }
    }
}