using Game.Scripts.View;
using Game.Scripts.View.UseCase;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Installer
{
    public class ViewInstaller : MonoInstaller
    {
        [SerializeField] private BaseProjectileView baseProjectileView;
        override public void InstallBindings()
        {
            BindSizeAndDensity();
        }

        private void BindSizeAndDensity()
        {
            Container.Bind<BaseProjectileView>().FromInstance(baseProjectileView).AsSingle();
            Container.BindInterfacesAndSelfTo<BaseProjectileUseCase>().AsSingle();
        }
    }
}