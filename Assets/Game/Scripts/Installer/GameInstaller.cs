using Assets.Game.Scripts.Core.Calculator;
using Assets.Game.Scripts.Infrastructure.Signals;
using Assets.Game.Scripts.Settings;
using DefaultNamespace;
using Game.Scripts.Core;
using Game.Scripts.Core.Force;
using Game.Scripts.Core.Simulation;
using Game.Scripts.Infrastructure.GameStateMachine;
using Game.Scripts.Infrastructure.GameStateMachine.GameState;
using Game.Scripts.Infrastructure.Signals;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindSettings();
        BindSignal();
        Container.BindInterfacesAndSelfTo<ProjectileFactory>().AsSingle().NonLazy();
        BindForce();
        BindCalculator();
        Container.Bind<IPhysicsIntegrator>().To<SemiImplicitEulerIntegrator>().AsSingle().NonLazy();

        Container.Bind<SimulationRecorder>().AsSingle();
        Container.Bind<SimulationPrinter>().AsSingle().NonLazy();
        Container.Bind<CsvExporter>().AsSingle();
        BindGameStateMachine();
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
    }

    private void BindForce()
    {
        Container.Bind<IForce>().To<GravityForce>().AsSingle().NonLazy();
        Container.Bind<IForce>().To<DragForce>().AsSingle().NonLazy();
    }

    private void BindGameStateMachine()
    {
        Container.Bind<GameStateMachine>().AsSingle().NonLazy();
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
    }
}