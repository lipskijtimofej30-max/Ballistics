using UnityEngine;

namespace Game.Scripts.Core
{
    public class ProjectileSettings
    {
        public ShapeType ShapeType { get; private set; } = ShapeType.Sphere;
        public float StartSpeed { get; private set; } = 15f;
        public float LaunchAngle { get; private set; } = 30f;
        public Vector3 StartPosition { get; private set; } = new Vector3(0f, 10.0f, 0.0f);
    }
}