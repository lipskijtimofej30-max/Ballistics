using UnityEngine;

namespace Game.Scripts.Core
{
    public class ProjectileSettings
    {
        public ShapeType ShapeType { get; private set; } = ShapeType.Sphere;
        public Vector3 StartVelocity { get; private set; } = new Vector3(30f, 0f, 0f);
        public Vector3 StartPosition { get; private set; } = new Vector3(0f, 10.0f, 0.0f);
    }
}