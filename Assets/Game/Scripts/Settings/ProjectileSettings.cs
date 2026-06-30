using UnityEngine;

namespace Game.Scripts.Core
{
    public class ProjectileSettings
    {
        public ShapeType ShapeType { get; set; } = ShapeType.Sphere;
        public float InitialSpeed { get; private set; } = 15f;
        public float LaunchAngle { get; private set; } = 30f;
        public Vector3 InitialPosition { get; private set; } = new Vector3(0f, 10.0f, 0.0f);
        public int Density { get; set; } = 7000;
        public float Size { get; set; } = 0.5f;
    }
}