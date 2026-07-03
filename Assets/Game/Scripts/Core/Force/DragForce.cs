using UnityEngine;
using Zenject;

namespace Game.Scripts.Core.Force
{
    public class DragForce : IForce
    {
        private EnvironmentSettings _environmentSettings;

        [Inject]
        private void Construct(EnvironmentSettings environmentSettings)
        {
            _environmentSettings = environmentSettings;
        }
        
        public Vector3 Calculate(Projectile projectile)
        {
            if(!_environmentSettings.AirResistanceEnabled)
                return Vector3.zero;
            
            var v_rel = projectile.Velocity - _environmentSettings.WindVelocity;
            return -0.5f * projectile.DragCoefficient * _environmentSettings.AirDensity * projectile.CrossSectionalArea * v_rel.magnitude *  v_rel;
        }
    }
}