using UnityEngine;

namespace Game.Scripts.Core
{
    public class MassCalculator
    {
        private ProjectileSettings _settings;
        
        public MassCalculator(ProjectileSettings settings)
        {
            _settings = settings;
        }
        
        public float GetMass(ShapeType shapeType)
        {
            float mass = 0f;
            if (shapeType == ShapeType.Cube)
                mass = _settings.Density * (_settings.Size * _settings.Size * _settings.Size); //m=p*a^3
            else if (shapeType == ShapeType.Sphere)
                mass = _settings.Density * (4f/3f * Mathf.PI * (_settings.Size * _settings.Size * _settings.Size)); //m=p(4/3пR^3)
            return mass;
        }
    }
}