using UnityEngine;
using Zenject;

namespace Game.Scripts.Core.Force
{
    public class GravityForce : IForce
    {
        private EnvironmentSettings _environmentSettings;

        [Inject]
        private void Construct(EnvironmentSettings environmentSettings)
        {
            _environmentSettings = environmentSettings;
        }

        public Vector3 Calculate(ProjectileState projectile)
        {
            return projectile.Mass * _environmentSettings.Gravity;
        }
    }
}