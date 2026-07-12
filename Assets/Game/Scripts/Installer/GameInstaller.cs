using Assets.Game.Scripts.Core.Calculator;
using Assets.Game.Scripts.Core.Experiment;
using Assets.Game.Scripts.Core.Experiment.Parameter;
using Assets.Game.Scripts.Core.Graphics;
using Assets.Game.Scripts.Infrastructure.GameStateMachine;
using Assets.Game.Scripts.Infrastructure.GameStateMachine.ExperimentState;
using Assets.Game.Scripts.Infrastructure.Signals;
using Assets.Game.Scripts.Settings;
using Assets.Game.Scripts.UX;
using DefaultNamespace;
using Game.Scripts.Core;
using Game.Scripts.Core.Force;
using Game.Scripts.Core.Simulation;
using Game.Scripts.Infrastructure.GameStateMachine;
using Game.Scripts.Infrastructure.GameStateMachine.GameState;
using Game.Scripts.Infrastructure.Signals;
using Game.Scripts.Settings;
using Game.Scripts.View.View;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Simulator _simulator;
    [SerializeField] private ExperimentPlaybackSequencer _sequencer;
    [SerializeField] private LaunchStand _launchStand;
    [SerializeField] private TrajectoryRenderer _prefab;
    [SerializeField] private GraphLine _linePrefab;
    public override void InstallBindings()
    {
        BindSettings();
        BindSignal();
        Container.Bind<Game.Scripts.Infrastructure.Logger.ILogger>().To<Game.Scripts.Infrastructure.Logger.Logger>().AsSingle().NonLazy();
        Container.Bind<ExperimentSession>().AsSingle().NonLazy();
        Container.Bind<GraphDataSourceFactory>().AsSingle();
        BindExperimentParameter();
        Container.Bind<ExperimentParameterDataBase>().AsSingle();
        Container.Bind<GraphLinePool>().AsSingle().WithArguments(_linePrefab);
        BindForce();
        BindCalculator();
        BindSimulation();
        BindSimulator();
        BindFactory();
        Container.Bind<GraphController>().AsSingle();
        Container.Bind<ExperimentRunner>().AsSingle().NonLazy();
        Container.Bind<TrajectoryPool>().AsSingle().WithArguments(_prefab);
        Container.Bind<ExperimentPlaybackController>().AsSingle();
        Container.Bind<ExperimentPlaybackSequencer>().FromInstance(_sequencer).AsSingle();
        Container.Bind<LaunchStand>().FromInstance(_launchStand).AsSingle();
        BindSimulationStateMachine();
        BindExperimentStateMachine();
        Container.BindInterfacesAndSelfTo<ModeController>().AsSingle();
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
        Container.Bind<DataExporter>().AsSingle();
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
        Container.Bind<ExperimentSettings>().AsSingle().NonLazy();
    }

    private void BindForce()
    {
        Container.Bind<IForce>().To<GravityForce>().AsSingle().NonLazy();
        Container.Bind<IForce>().To<DragForce>().AsSingle().NonLazy();
    }

    private void BindSimulationStateMachine()
    {
        Container.Bind<GameStateMachine<SimulationStateType>>().AsSingle();
        Container.Bind<SimulationSetupState>().AsSingle();
        Container.Bind<SimulationState>().AsSingle();
        Container.Bind<SimulationFinishedState>().AsSingle();
        Container.Bind<SimulationPausedState>().AsSingle();
        Container.Bind<SimulationController>().AsSingle().NonLazy();
    }

    private void BindExperimentStateMachine()
    {
        Container.Bind<GameStateMachine<ExperimentStateType>>().AsSingle();
        Container.Bind<ExperimentSetupState>().AsSingle();
        Container.Bind<ExperimentRunningState>().AsSingle();
        Container.Bind<ExperimentPauseState>().AsSingle();
        Container.Bind<ExperimentFinishedState>().AsSingle();
        Container.Bind<ExperimentController>().AsSingle().NonLazy();
    }

    private void BindExperimentParameter()
    {
        Container.Bind<IExperimentParameter>().To<InitialSpeedParameter>().AsSingle();
        Container.Bind<IExperimentParameter>().To<LaunchAngleParameter>().AsSingle();
    }

    private void BindSignal()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<ChangeStateSignal<SimulationStateType>>().OptionalSubscriber();
        Container.DeclareSignal<ChangeStateSignal<ExperimentStateType>>().OptionalSubscriber();
        Container.DeclareSignal<ConfirmButtonClickSignal>().OptionalSubscriber();
        Container.DeclareSignal<ProjectileSettingsChangedSignal>().OptionalSubscriber();
        Container.DeclareSignal<SimulationSettingsChangedSignal>().OptionalSubscriber();
        Container.DeclareSignal<EnvironmentSettingsChangedSignal>().OptionalSubscriber();
        Container.DeclareSignal<ProjectileSpawnedSignal>().OptionalSubscriber();
        Container.DeclareSignal<VisualizationSettingsChangedSignal>().OptionalSubscriber();
        Container.DeclareSignal<ShapeDropdownChangedSignal>().OptionalSubscriber();
        Container.DeclareSignal<IntegratorSettingsChangedSignal>().OptionalSubscriber();
        Container.DeclareSignal<SetupDirtyStatusChangedSignal>().OptionalSubscriber();
        Container.DeclareSignal<CleanSetupRequestedSignal>().OptionalSubscriber();
        Container.DeclareSignal<ChangeAppModeSignal>().OptionalSubscriber();
    }
}