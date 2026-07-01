using Game.Scripts.View;
using Game.Scripts.View.UseCase;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Installer
{
    public class ViewInstaller : MonoInstaller
    {
        [SerializeField] private ProjectileView projectileView;
        override public void InstallBindings()
        {
            BindSizeAndDensity();
        }

        private void BindSizeAndDensity()
        {
            Container.Bind<ProjectileView>().FromInstance(projectileView).AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectileUseCase>().AsSingle();
        }
    }
}