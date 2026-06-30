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
        private ProjectileSettings _settings;
        
        public void Initialize(ProjectileSettings gameSettings, LaunchVelocityCalculator velocityCalculator, MassCalculator massCalculator)
        {
            _settings = gameSettings;
            
            Velocity = velocityCalculator.GetVelocity();
            Position = _settings.InitialPosition;
            transform.position = Position;
            
            Mass = massCalculator.GetMass(ShapeType);
            CrossSectionalArea = GetCrossSectionalArea();
            DragCoefficient = GetDragCoefficient();
            
            Debug.Log($"Shape name : {gameObject.name}, shape type: {ShapeType}, mass : {Mass}, dragCoefficient: {DragCoefficient}, cross sectional area : {CrossSectionalArea};\n start speed : {_settings.InitialSpeed}, start velocity : {Velocity}, start velocity magnitude : {Velocity.magnitude}");
        }
        
        private float GetCrossSectionalArea()
        {
            float area = 0f;
            if (ShapeType == ShapeType.Cube)
                area =_settings.Size * _settings.Size;
            else if (ShapeType == ShapeType.Sphere)
                area = Mathf.PI * (_settings.Size * _settings.Size);
            return area;
        }

        private float GetDragCoefficient()
        {
            float dragCoefficient = 0f;
            if (ShapeType == ShapeType.Cube)
                dragCoefficient = 1.05f;
            else if (ShapeType == ShapeType.Sphere)
                dragCoefficient = 0.47f;
            return dragCoefficient;
        }
    }
    
    public enum ShapeType
    {
        Cube,
        Sphere
    }
}