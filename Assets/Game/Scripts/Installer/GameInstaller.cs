using Assets.Game.Scripts.Core.Calculator;
using Assets.Game.Scripts.Infrastructure.Signals;
using Assets.Game.Scripts.Settings;
using DefaultNamespace;
using Game.Scripts.Core;
using Game.Scripts.Core.Force;
using Game.Scripts.Core.Simulation;
using Game.Scripts.Infrastructure.GameStateMachine;
using Game.Scripts.Infrastructure.GameStateMachine.GameState;
using Game.Scripts.Infrastructure.Logger;
using Game.Scripts.Infrastructure.Signals;
using Game.Scripts.Settings;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Simulator _simulator;
    public override void InstallBindings()
    {
        BindSettings();
        Container.Bind<Game.Scripts.Infrastructure.Logger.ILogger>().To<Game.Scripts.Infrastructure.Logger.Logger>().AsSingle().NonLazy();
        BindSignal();
        BindForce();
        BindCalculator();
        BindSimulation();
        BindSimulator();
        BindFactory();
        Container.Bind<TrajectoryPool>().AsSingle();
        BindGameStateMachine();
    }

    private void BindFactory()
    {
        Container.BindInterfacesAndSelfTo<ProjectileFactory>().AsSingle().NonLazy();
        Container.Bind<IntegratorFactory>().AsSingle();
    }

    private void BindSimulator()
    {
        Container.Bind<Simulator>().FromInstance(_simulator).AsSingle().NonLazy();
        Container.Bind<FastForwardSimulator>().AsSingle();
    }

    private void BindSimulation()
    {
        Container.Bind<SimulationPrinter>().AsSingle().NonLazy();
        Container.Bind<CsvExporter>().AsSingle();
        Container.Bind<SimulationAnalyzer>().AsSingle();
        Container.Bind<SimulationExporter>().AsSingle();
    }

    private void BindCalculator()
    {
        Container.Bind<LaunchVelocityCalculator>().AsSingle();
        Container.Bind<MassCalculator>().AsSingle();
        Container.Bind<ForceCalculator>().AsSingle().NonLazy();
        Container.Bind<CrossSectionalAreaCalculator>().AsSingle();
    }

    private void BindSettings()
    {
        Container.Bind<EnvironmentSettings>().AsSingle().NonLazy();
        Container.Bind<ProjectileSettings>().AsSingle().NonLazy();
        Container.Bind<SimulationSettings>().AsSingle().NonLazy();
        Container.Bind<VisualizationSettings>().AsSingle().NonLazy();
        Container.Bind<IntegratorSettings>().AsSingle().NonLazy();
    }

    private void BindForce()
    {
        Container.Bind<IForce>().To<GravityForce>().AsSingle().NonLazy();
        Container.Bind<IForce>().To<DragForce>().AsSingle().NonLazy();
    }

    private void BindGameStateMachine()
    {
        Container.Bind<GameStateMachine>().AsSingle();
        Container.Bind<SetupSimulationState>().AsSingle();
        Container.Bind<SimulationState>().AsSingle();
        Container.Bind<FinishedSimulationState>().AsSingle();
        Container.Bind<PausedSimulationState>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameController>().AsSingle().NonLazy();
    }

    private void BindSignal()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<ChangeStateSignal>().OptionalSubscriber();
        Container.DeclareSignal<ConfirmButtonClickSignal>().OptionalSubscriber();
        Container.DeclareSignal<ProjectileSettingsChangedSignal>().OptionalSubscriber();
        Container.DeclareSignal<SimulationSettingsChangedSignal>().OptionalSubscriber();
        Container.DeclareSignal<EnvironmentSettingsChangedSignal>().OptionalSubscriber();
        Container.DeclareSignal<ProjectileSpawnedSignal>().OptionalSubscriber();
        Container.DeclareSignal<VisualizationSettingsChangedSignal>().OptionalSubscriber();
        Container.DeclareSignal<ShapeDropdownChangedSignal>().OptionalSubscriber();
        Container.DeclareSignal<IntegratorSettingsChangedSignal>().OptionalSubscriber();
    }
}