using Game.Scripts.Core;
using Game.Scripts.Core.Force;
using Game.Scripts.Core.Simulation;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindSettings();
        Container.BindInterfacesAndSelfTo<ProjectileFactory>().AsSingle().NonLazy();
        BindForce();
        Container.Bind<ForceCalculator>().AsSingle().NonLazy();
        Container.Bind<PhysicsIntegrator>().AsSingle().NonLazy();
        
        Container.Bind<SimulationRecorder>().AsSingle();
        Container.Bind<SimulationPrinter>().AsSingle().NonLazy();
        Container.Bind<CsvExporter>().AsSingle();
    }

    private void BindSettings()
    {
        Container.Bind<EnvironmentSettings>().AsSingle().NonLazy();
        Container.Bind<ProjectileSettings>().AsSingle().NonLazy();
    }

    private void BindForce()
    {
        Container.Bind<IForce>().To<GravityForce>().AsSingle().NonLazy();
        Container.Bind<IForce>().To<DragForce>().AsSingle().NonLazy();
    }
}