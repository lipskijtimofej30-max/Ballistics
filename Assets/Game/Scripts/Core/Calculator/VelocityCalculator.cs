using UnityEngine;

namespace Game.Scripts.Core
{
    public class VelocityCalculator
    {
        public Vector3 GetVelocity(float initialSpeed, float launchAngle)
        {
            float angle = Mathf.Deg2Rad * launchAngle;
            var velocity = new Vector3(
                initialSpeed * Mathf.Cos(angle),
                initialSpeed * Mathf.Sin(angle),
                0f);
            return velocity;
        }

        public float GetAngleForVelocity(Vector3 velocity)
        {
            return Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        }
    }
}