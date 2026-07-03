using Assets.Game.Scripts.Core.Calculator;
using Assets.Game.Scripts.Settings;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class Projectile : MonoBehaviour
    { 
        [field: SerializeField] public ShapeType ShapeType { get; private set; }
        
        public float Mass { get; private set; }
        public float CrossSectionalArea { get; private set;}
        public float DragCoefficient { get; private set; }

        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }

        
        public void Initialize(Vector3 initialPosition, Vector3 initialVelocity,float mass, float area, float dragCoefficient)
        {
            
            Velocity = initialVelocity;
            Position = initialPosition;
            transform.position = Position;
            
            Mass = mass;
            CrossSectionalArea = area;
            DragCoefficient = dragCoefficient;

            Debug.Log(
                $"Shape name : {gameObject.name}, shape type: {ShapeType}, mass : {Mass}, dragCoefficient: {DragCoefficient}, cross sectional area : {CrossSectionalArea}.");
        }
    }
    
    public enum ShapeType
    {
        Cube,
        Sphere
    }
}