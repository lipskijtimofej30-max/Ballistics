using Assets.Game.Scripts.Infrastructure.Signals;
using Assets.Game.Scripts.Settings;
using System;
using Game.Scripts.Core;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.View.UseCase
{
    public class SimulationUseCase : IDisposable
    {
        private readonly FloatParameterBinder _speedBinder;
        private readonly FloatParameterBinder _heightBinder;
        private readonly FloatParameterBinder _angleBinder;

        public SimulationUseCase(
            SignalBus signalBus,
            SimulationSettings settings,
            SimulationView view)
        {
            _speedBinder = new FloatParameterBinder(
                view.InitialSpeedParameter,
                0.1f,
                50f,
                "F2",
                () => settings.InitialSpeed,
                x => settings.InitialSpeed = x,
                () => signalBus.Fire<SimulationSettingsChangedSignal>());

            _heightBinder = new FloatParameterBinder(
                view.HeightParameter,
                0.1f,
                15f,
                "F2",
                () => settings.InitialPosition.y,
                x => settings.InitialPosition = new Vector3(0, x, 0),
                () => signalBus.Fire<SimulationSettingsChangedSignal>());

            _angleBinder = new FloatParameterBinder(
                view.AngleParameter,
                0.1f,
                90f,
                "F2",
                () => settings.LaunchAngle,
                x => settings.LaunchAngle = x,
                () => signalBus.Fire<SimulationSettingsChangedSignal>());
        }

        public void Dispose()
        {
            _speedBinder.Dispose();
            _heightBinder.Dispose();
            _angleBinder.Dispose();
        }
    }
}
