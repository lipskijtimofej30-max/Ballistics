using System;
using Zenject;

namespace Game.Scripts.Core
{
    public class IntegratorFactory
    {
        private readonly DiContainer _container;

        [Inject]
        public IntegratorFactory(DiContainer container)
        {
            _container = container;
        }

        public IPhysicsIntegrator Create(IntegratorMethod method) => method switch
        {
            IntegratorMethod.Euler => _container.Instantiate<EulerIntegrator>(),
            IntegratorMethod.SemiImplicitEuler => _container.Instantiate<SemiImplicitEulerIntegrator>(),
            IntegratorMethod.RK2 => _container.Instantiate<RK2Integrator>(),
            IntegratorMethod.RK4 => _container.Instantiate<RK4Integrator>(),
            IntegratorMethod.VelocityVerlet => _container.Instantiate<VelocityVerletIntegrator>(),
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, null)
        };
    }
}