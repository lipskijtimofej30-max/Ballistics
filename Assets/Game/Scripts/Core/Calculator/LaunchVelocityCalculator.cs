using UnityEngine;

namespace Game.Scripts.Core
{
    public class LaunchVelocityCalculator
    {
        private ProjectileSettings _projectileSettings;

        public LaunchVelocityCalculator(ProjectileSettings projectileSettings)
        {
            _projectileSettings = projectileSettings;
        }

        public Vector3 GetVelocity()
        {
            float angle = Mathf.Deg2Rad * _projectileSettings.LaunchAngle;
            var velocity = new Vector3(
                _projectileSettings.InitialSpeed * Mathf.Cos(angle),
                _projectileSettings.InitialSpeed * Mathf.Sin(angle),
                0f);
            return velocity;
        }
    }
}