using Assets.Game.Scripts.View;
using Assets.Game.Scripts.View.UseCase;
using Game.Scripts.View;
using Game.Scripts.View.UseCase;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Installer
{
    public class ViewInstaller : MonoInstaller
    {
        [SerializeField] private ProjectileView projectileView;
        [SerializeField] private SimulationView simulationView;
        [SerializeField] private EnvironmentView environmentView;
        override public void InstallBindings()
        {
            BindProjectileView();
            BindSimulationView();
            BindEnvironmentView();
        }

        private void BindEnvironmentView()
        {
            Container.Bind<EnvironmentView>().FromInstance(environmentView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<EnvironmentUseCase>().AsSingle().NonLazy();
        }

        private void BindProjectileView()
        {
            Container.Bind<ProjectileView>().FromInstance(projectileView).AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectileUseCase>().AsSingle();
        }

        private void BindSimulationView()
        {
            Container.Bind<SimulationView>().FromInstance(simulationView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SimulationUseCase>().AsSingle().NonLazy();
        }
    }
}