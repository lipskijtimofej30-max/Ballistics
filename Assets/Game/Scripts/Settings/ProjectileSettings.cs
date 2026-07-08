using UnityEngine;

namespace Game.Scripts.Core
{
    public class ProjectileSettings
    {
        public ShapeType ShapeType { get; set; } = ShapeType.Sphere;
      
        public float Density { get; set; } = 7000f;
        public float Size { get; set; } = 0.5f;

        public ProjectileSettings Clone() => new ProjectileSettings
        {
            ShapeType = ShapeType,
            Density = Density,
            Size = Size
        };
    }
}