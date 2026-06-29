using Game.Scripts.Core;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameSettings>().AsSingle().NonLazy();
        
        Container.BindInterfacesAndSelfTo<ProjectileFactory>().AsSingle().NonLazy();
        
        Container.Bind<ForceCalculator>().AsSingle().NonLazy();
    }
}