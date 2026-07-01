using Game.Scripts.Core;
using UnityEngine;

namespace Assets.Game.Scripts.Core.Calculator
{
    public class CrossSectionalAreaCalculator
    {
        public float GetCrossSectionalArea(ShapeType shapeType, float size)
        {
            float area = 0;
            if (shapeType == ShapeType.Cube)
                area = size * size;
            else if (shapeType == ShapeType.Sphere)
                area = Mathf.PI * (size * size);
            return area;
        }
    }
}
