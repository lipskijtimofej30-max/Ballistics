using System;
using System.Collections;
using Assets.Game.Scripts.Infrastructure.GameStateMachine;
using Assets.Game.Scripts.Settings;
using Assets.Game.Scripts.UX;
using DefaultNamespace;
using Game.Scripts.Core;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Core.Experiment
{
    public class ExperimentPlaybackSequencer : MonoBehaviour
    {
        private ExperimentSession _session;
        private ExperimentSettings _experimentSettings;
        private ExperimentPlaybackController _playbackController;
        private TrajectoryPool _trajectoryPool;
        private SignalBus _signalBus;

        private LaunchStand _launchStand;

        private Coroutine _routine;
        private Action<Vector3> _currentPositionCallback;
        
        public event Action RunPlaybackStarted;
        public event Action<ExperimentRunResult> RunPlaybackFinished;

        [Inject]
        private void Construct(
            ExperimentSession session,
            ExperimentPlaybackController playbackController,
            TrajectoryPool trajectoryPool,
            SignalBus signalBus,
            ExperimentSettings experimentSettings,
            LaunchStand launchStand)
        {
            _session = session;
            _playbackController = playbackController;
            _trajectoryPool = trajectoryPool;
            _signalBus = signalBus;
            _experimentSettings = experimentSettings;
            _launchStand = launchStand;
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

            _playbackController.Stop();
            
            if (_currentPositionCallback != null)
            {
                _playbackController.PositionUpdated -= _currentPositionCallback;
                _currentPositionCallback = null;
            }
        }

        private IEnumerator PlaySeriesRoutine()
        {
            foreach (var result in _session.ExperimentRunResults)
            {
                float height = result.Preset.InitialHeight;
                float angle = result.Preset.LaunchAngle;
                float radius = result.Preset.Size;

                Vector3 startPos = new (0f, height, 0f);
                
                _launchStand.SetParameters(height, angle, radius, startPos);
                
                var renderer = _trajectoryPool.Rent();
                renderer.SetVisible(true);
                renderer.SetColor(result.RunId);
                renderer.Clear();
                
                _currentPositionCallback = pos => renderer.AppendPoint(pos); 
                _playbackController.PositionUpdated += _currentPositionCallback;
                
                RunPlaybackStarted?.Invoke();
                
                _playbackController.Play(result.RunId, result.Run);
                
                yield return new WaitUntil(() => !_playbackController.IsPlaying);

                if (_currentPositionCallback != null)
                {
                    _playbackController.PositionUpdated -= _currentPositionCallback;
                    _currentPositionCallback = null;
                }
                RunPlaybackFinished?.Invoke(result);
                yield return new WaitForSeconds(_experimentSettings.PauseBetweenRuns);;
            }
            
            _signalBus.Fire(new ChangeStateSignal<ExperimentStateType>(ExperimentStateType.Finished));
            _routine = null;
        }
    }
}