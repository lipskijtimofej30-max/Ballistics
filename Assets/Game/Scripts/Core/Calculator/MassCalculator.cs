using UnityEngine;

namespace Game.Scripts.Core
{
    public class MassCalculator
    {
        public float GetMass(ShapeType shapeType, float density, float size)
        {
            float mass = 0f;
            if (shapeType == ShapeType.Cube)
                mass = density * (size * size * size); //m=p*a^3
            else if (shapeType == ShapeType.Sphere)
                mass = density * (4f/3f * Mathf.PI * (size * size * size)); //m=p(4/3пR^3)
            return mass;
        }
    }
}